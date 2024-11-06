using UnityEngine;
using Core.UI;
using UnityEngine.UI;
using PhotonEngine.PhotonRoom;
using Core.ServiceLocator;
using LoadingScreenScene;

namespace PlayerUIScene.SideMenu.Controllers
{
    public sealed class ExitController : UIController
    {
        [SerializeField] private Button _exitButton;

        #region Services

        private ILoadingScreenService _loadingScreenInstance;
        private ILoadingScreenService _loadingScreen
            => _loadingScreenInstance ??= Service.Instance.Get<ILoadingScreenService>();


        private INetworkService _photonRoomInstance;
        private INetworkService _photonRoom => _photonRoomInstance ??= Service.Instance.Get<INetworkService>();

        #endregion

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(Exit);
        }

        protected override void Start()
        {
            Show();
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(Exit);
        }

        private void Exit()
        {
            _loadingScreen.OnScreenLoaded.AddListener(ExitFromRoom);
            _loadingScreen.Show();
        }

        private void ExitFromRoom()
        {
            _loadingScreen.OnScreenLoaded.RemoveListener(ExitFromRoom);
            _photonRoom.ExitFromNetworkRoom();
        }
    }
}