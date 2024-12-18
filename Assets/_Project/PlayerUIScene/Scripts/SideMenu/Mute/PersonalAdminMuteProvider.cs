using Photon.Realtime;
using Photon.Pun;
using Core.ServiceLocator;

namespace PlayerUIScene.SideMenu.Mute
{
    public sealed class PersonalAdminMuteProvider : MonoBehaviourPunCallbacks
    {
        #region Services

        private IPersonalAdminMuteService _personalAdminMuteInstance;
        private IPersonalAdminMuteService _personalAdminMute
            => _personalAdminMuteInstance ??= Service.Instance.Get<IPersonalAdminMuteService>();

        #endregion

        public override void OnEnable()
        {
            base.OnEnable();
            _personalAdminMute.AddPhotonEventReceiver();
        }

        private void Start()
        {
            _personalAdminMute.CheckPlayersMuteProperty();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _personalAdminMute.RemovePhotoEventReceiver();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            _personalAdminMuteInstance.OnPlayerLeftRoom(otherPlayer);
        }
    }
}