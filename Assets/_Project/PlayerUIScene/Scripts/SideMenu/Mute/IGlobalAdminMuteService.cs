using Core.ServiceLocator;

namespace PlayerUIScene.SideMenu.Mute
{
    public interface IGlobalAdminMuteService : IService
    {
        bool IsGlobalMuteEnabled();
        void SetGlobalMute();
        void SetGlobalUnmute();
        void OnMuteAllButtonClick();
        void OnUnmuteAllButtonClick();
    }
}