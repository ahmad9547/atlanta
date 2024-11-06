using Core.ServiceLocator;
using PlayerUIScene.SideMenu.Enums;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.PlayerUIScene.Scripts.SideMenu.Services
{
    public interface IMicrophoneStateService : IService
    {
        UnityEvent SetMutedByAdminEvent { get; }
        UnityEvent SetUnmutedByAdminEvent { get; }
        MuteState PlayerLastMuteState { get; }

        void SetMutedByAdmin();
        void SetUnmutedByAdmin();
        void SetMuteState(Button muteButton);
        void SetUnmuteState(Button muteButton);
    }
}