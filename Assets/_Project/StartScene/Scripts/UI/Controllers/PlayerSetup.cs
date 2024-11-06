using UnityEngine;
using UnityEngine.UI;
using Core.UI;
using LocationsManagement.Interfaces;
using Core.ServiceLocator;
using LoadingScreenScene;
using ProjectConfig.Session;
using ProjectConfig.Session.Services;
using StartScene.Services;
using UserManagement;

namespace StartScene.UI.Controllers
{
    public sealed class PlayerSetup : UIController
    {
        [SerializeField] private Button _goButton;
        [SerializeField] private GameObject _atlantaPreview;

        #region Services

        private IUserProfileService _userProfileService;
        private IUserProfileService _userProfile
            => _userProfileService ??= Service.Instance.Get<IUserProfileService>();

        private ISessionConfigHandler _sessionConfigHandlerService;
        private ISessionConfigHandler _sessionConfigHandler
            => _sessionConfigHandlerService ??= Service.Instance.Get<ISessionConfigHandler>();

        private ILocationLoaderService _locationLoaderInstance;
        private ILocationLoaderService _locationLoader =>
            _locationLoaderInstance ??= Service.Instance.Get<ILocationLoaderService>();

        private ILoadingScreenService _loadingScreenInstance;
        private ILoadingScreenService _loadingScreen
            => _loadingScreenInstance ??= Service.Instance.Get<ILoadingScreenService>();

        private IStartSceneStateHolderService _startSceneStateHolderService;
        private IStartSceneStateHolderService _startSceneStateHolder
            => _startSceneStateHolderService ??= Service.Instance.Get<IStartSceneStateHolderService>();

        #endregion

        private void OnEnable()
        {
            _goButton.onClick.AddListener(OnGoButtonPressed);
            _sessionConfigHandler.OnSessionConfigInitialized.AddListener(StartUserExperience);
        }

        protected override void Start()
        {
            bool wasAlreadyLoaded = _startSceneStateHolder.WasAlreadyLoaded;
            _atlantaPreview.SetActive(wasAlreadyLoaded);
            _goButton.interactable = wasAlreadyLoaded;

            Show();

            if (!wasAlreadyLoaded)
            {
                _startSceneStateHolder.WasAlreadyLoaded = true;
                ShowLoadingScreen();
            }
        }

        private void OnDisable()
        {
            _goButton.onClick.RemoveListener(OnGoButtonPressed);
            _sessionConfigHandler.OnSessionConfigInitialized.RemoveListener(StartUserExperience);
            _loadingScreen.OnScreenLoaded.RemoveListener(OnScreenLoaded);
        }

        private void OnGoButtonPressed()
        {
            _goButton.interactable = false;

            _uiAnimator.ButtonScale(_goButton, () =>
            {
                StartUserExperience();
                Hide();
            });
        }

        private void StartUserExperience()
        {
            _userProfile.SetupUserProfile(new UserModel()
            {
                Nickname = SessionConfig.Nickname,
                AvatarId = SessionConfig.AvatarLink,
                IsAdmin = SessionConfig.AdminStatus
            });

            _locationLoader.LoadFirstLocation();
        }

        private void ShowLoadingScreen()
        {
            _loadingScreen.OnScreenLoaded.AddListener(OnScreenLoaded);
            _loadingScreen.Show();
        }

        private void OnScreenLoaded()
        {
            _loadingScreen.OnScreenLoaded.RemoveListener(OnScreenLoaded);
            _loadingScreen.SetLoadingProgressMessage("Loading session data...");
        }
    }
}