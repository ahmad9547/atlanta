using UnityEngine;
using PhotonEngine;
using LoadingScreenScene;
using Core.ServiceLocator;
using LocationsManagement.Interfaces;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using PhotonEngine.PhotonRoom;
using LocationsManagement.Enums;
using Metaverse.Teleport.Interfaces;
using ProjectConfig.Session;

namespace LocationsManagement.Services
{
    public sealed class LocationsLoader : ILocationLoaderService, INetworkCallbacks
    {
        private const string StartScene = "StartScene";

        public Location LoadedLocation { get; private set; }

        #region Services
        private ILoadingScreenService _loadingScreenInstance;

        private ILoadingScreenService _loadingScreen
            => _loadingScreenInstance ??= Service.Instance.Get<ILoadingScreenService>();

        private IAddressablesLoaderService _addressablesLoaderInstance;

        private IAddressablesLoaderService _addressablesLoader
            => _addressablesLoaderInstance ??= Service.Instance.Get<IAddressablesLoaderService>();

        private INetworkService _networkInstance;

        private INetworkService _network
            => _networkInstance ??= Service.Instance.Get<INetworkService>();

        private ILocationsHolderService _locationsHolderInstance;

        private ILocationsHolderService _locationsHolder
            => _locationsHolderInstance ??= Service.Instance.Get<ILocationsHolderService>();

        private IStartupPointHolderService _startupPointHolderInstance;
        private IStartupPointHolderService _startupPointHolder
            => _startupPointHolderInstance ??= Service.Instance.Get<IStartupPointHolderService>();

        #endregion

        private LocationType _nextLocationType;

        private bool _isPreviousLocationUnloadingProcessActive = false;

        private bool isSwitchingScene = false;

        public void LoadFirstLocation()
        {
            _network.AddNetworkRoomCallbacksObserver(this);

            _loadingScreen.OnScreenLoaded.AddListener(OnFirstLocationLoadingScreenShowed);

            _loadingScreen.Show();
        }

        public void ChangeLocation(LocationType nextLocationType)
        {
            _isPreviousLocationUnloadingProcessActive = true;

            _nextLocationType = nextLocationType;

            _loadingScreen.OnScreenLoaded.AddListener(OnChangeLocationScreenLoaded);

            _loadingScreen.Show();
        }

        public void OnConnectedToNetworkLobby()
        {
            Debug.Log("Connected to Network Lobby");

            JoinNetworkLocationRoom();
        }

        public void LoadEnvironment()
        {
            if (_isPreviousLocationUnloadingProcessActive)
            {
                UnloadPreviousLocation();
                return;
            }

            _startupPointHolder.StartupPointType = SessionConfig.TeleportPointId;
            LoadedLocation = _locationsHolder.GetFirstLocation();
            LoadLocationEnvironment();
        }

        public void OnJoinedNetworkRoom()
        {
            LoadLocationFunctionality();
        }

        public void OnJoinedNetworkRoomFailed()
        {
            JoinNetworkLocationRoom();
        }

        public void OnDisconnectedFromNetworkServer()
        {
            if(isSwitchingScene)
            {
                isSwitchingScene = false;
                return;
            }

            _loadingScreen.OnScreenLoaded.AddListener(OnStartMenuLoadingScreenShowed);

            _loadingScreen.Show();
        }

        private void OnFirstLocationLoadingScreenShowed()
        {
            SceneManager.UnloadSceneAsync(StartScene).completed
                += OnStartSceneUnloaded;

            _loadingScreen.OnScreenLoaded.RemoveListener(OnFirstLocationLoadingScreenShowed);
        }

        private void OnStartSceneUnloaded(AsyncOperation handle)
        {
            handle.completed -= OnStartSceneUnloaded;
            Debug.Log("Unloading Start Scene");

            LoadEnvironment();
        }

        private void ConnectToServer()
        {
            _loadingScreen.SetLoadingProgressMessage("Connecting to server..");

            _network.Connect();
        }

        private void OnStartMenuLoadingScreenShowed()
        {
            BackToStartMenu();

            _loadingScreen.OnScreenLoaded.RemoveListener(OnStartMenuLoadingScreenShowed);
        }

        private async void LoadLocationEnvironment()
        {
            _loadingScreen.SetLoadingProgressMessage($"Loading {LoadedLocation.LocationName} environment..");

            await _addressablesLoader.Initialize();

            await ChangeScene();

            ConnectToServer();
        }

        private void JoinNetworkLocationRoom()
        {
            _loadingScreen.SetLoadingProgressMessage("Entering Multiuser Mode..");

            _network.JoinOrCreateNetworkRoom(LoadedLocation.LocationType.ToString());
        }

        private async Task ChangeScene()
        {
            SceneInstance locationSceneInstance = await _addressablesLoader
                .LoadSceneFromAssetReference(LoadedLocation.LocationReference);

            LoadedLocation.LocationSceneInstance = locationSceneInstance;

            SceneManager.SetActiveScene(locationSceneInstance.Scene);
        }

        private void LoadLocationFunctionality()
        {
            _loadingScreen.SetLoadingProgressMessage("Creating player..");

            Object.Instantiate(LoadedLocation.LocationFunctionalityPrefab);
        }

        private async void BackToStartMenu()
        {
            await UnloadLoadedLocation();

            _loadingScreen.SetLoadingProgressMessage($"Loading avatars menu..");

            SceneManager.LoadSceneAsync(StartScene, LoadSceneMode.Additive)
                .completed += OnStartSceneLoaded;
        }

        private async Task UnloadLoadedLocation()
        {
            if (LoadedLocation == null)
            {
                return;
            }

            _loadingScreen.SetLoadingProgressMessage($"Unloading {LoadedLocation.LocationName} environment..");

            await _addressablesLoader.UnloadScene(LoadedLocation.LocationSceneInstance);

            LoadedLocation.CleanAssetReferences();
        }

        private void OnStartSceneLoaded(AsyncOperation handle)
        {
            _network.RemoveNetworkRoomCallbacksObserver(this);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(StartScene));

            Resources.UnloadUnusedAssets()
            .completed += OnUnusedAssetsUnloaded;

            handle.completed -= OnStartSceneLoaded;
        }

        private void OnUnusedAssetsUnloaded(AsyncOperation handle)
        {
            _loadingScreen.Hide();

            handle.completed -= OnUnusedAssetsUnloaded;
        }

        private void OnChangeLocationScreenLoaded()
        {
            _loadingScreen.OnScreenLoaded.RemoveListener(OnChangeLocationScreenLoaded);
            isSwitchingScene = true;
            _network.ExitFromNetworkRoom();
            LoadEnvironment();
        }

        private async void UnloadPreviousLocation()
        {
            await UnloadLoadedLocation();

            Resources.UnloadUnusedAssets()
            .completed += OnPreviousLocationUnloaded;
        }

        private void OnPreviousLocationUnloaded(AsyncOperation handle)
        {
            handle.completed -= OnPreviousLocationUnloaded;

            LoadedLocation = _locationsHolder.GetLocationByType(_nextLocationType);

            LoadLocationEnvironment();

            _isPreviousLocationUnloadingProcessActive = false;
        }
    }
}
