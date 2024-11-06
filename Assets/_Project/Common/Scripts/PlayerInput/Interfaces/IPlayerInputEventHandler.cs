using Core.ServiceLocator;
using UnityEngine.Events;

namespace Common.PlayerInput.Interfaces
{
    public interface IPlayerInputEventHandler : IService
    {
        UnityEvent OnSideMenuSwitch { get; }

        UnityEvent OnPlaySpeakerAnimation { get; }

        UnityEvent OnSwipePresentationLeft { get; }

        UnityEvent OnSwipePresentationRight { get; }

        UnityEvent OnPlayPausePresentationVideo { get; }

        UnityEvent OnPresentationVideoReset { get; }

        UnityEvent OnCameraViewSwitch { get; }

        UnityEvent OnInteraction { get; }

        void SwitchSideMenu();

        void PlaySpeakerAnimation();

        void SwipePresentationSlideLeft();

        void SwipePresentationSlideRight();

        void PlayPausePresentationVideo();

        void ResetPresentationVideo();

        void SwitchCameraView();

        void Interact();
    }
}