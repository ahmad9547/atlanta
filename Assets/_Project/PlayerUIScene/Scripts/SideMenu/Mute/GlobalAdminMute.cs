using _Project.PlayerUIScene.Scripts.SideMenu.Services;
using Core.ServiceLocator;
using Photon.Realtime;
using PhotonEngine.CustomProperties.Enums;
using PhotonEngine.CustomProperties.Services;
using PhotonEngine.PhotonEvents;
using PhotonEngine.PhotonEvents.Enums;

namespace PlayerUIScene.SideMenu.Mute
{
    public class GlobalAdminMute : IGlobalAdminMuteService
    {
        private bool _globalMuteEnabled;

        #region Services

        private IPersonalAdminMuteService _personalAdminMuteInstance;
        private IPersonalAdminMuteService _personalAdminMute
            => _personalAdminMuteInstance ??= Service.Instance.Get<IPersonalAdminMuteService>();

        private IUserListService _userListInstance;
        private IUserListService _userList
            => _userListInstance ??= Service.Instance.Get<IUserListService>();

        private IPhotonEventsSenderService _photonEventsSenderInstance;
        private IPhotonEventsSenderService _photonEventsSender
            => _photonEventsSenderInstance ??= Service.Instance.Get<IPhotonEventsSenderService>();

        private IRoomCustomPropertiesService _roomCustomPropertiesService;
        private IRoomCustomPropertiesService _roomCustomProperties
            => _roomCustomPropertiesService ??= Service.Instance.Get<IRoomCustomPropertiesService>();

        #endregion

        public bool IsGlobalMuteEnabled()
        {
            return _globalMuteEnabled;
        }

        public void SetGlobalMute()
        {
            _globalMuteEnabled = true;

            _personalAdminMute.SetPersonalMuteForEachPlayer();

            _userList.UpdatePlayers();
        }

        public void SetGlobalUnmute()
        {
            _globalMuteEnabled = false;

            _personalAdminMute.SetPersonalUnmuteForEachPlayer();

            _userList.UpdatePlayers();
        }

        public void OnMuteAllButtonClick()
        {


            _roomCustomProperties.AddOrUpdateRoomCustomProperty(RoomCustomPropertyKey.AllAreMuted.ToString());

            _photonEventsSender.SendPhotonEvent(PhotonEventCode.GlobalMuteEventCode, ReceiverGroup.Others);
        }

        public void OnUnmuteAllButtonClick()
        {

            _roomCustomProperties.RemoveRoomCustomProperty(RoomCustomPropertyKey.AllAreMuted.ToString());

            _photonEventsSender.SendPhotonEvent(PhotonEventCode.GlobalUnmuteEventCode, ReceiverGroup.Others);
        }
    }
}