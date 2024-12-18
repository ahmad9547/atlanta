using Common.PlayerInput.Interfaces;
using UnityEngine.Events;

namespace Common.PlayerInput.Services
{
    public sealed class PlayerInputEventHandler : IPlayerInputEventHandler
    {
        public UnityEvent OnSideMenuSwitch { get; } = new UnityEvent();

        public UnityEvent OnPlaySpeakerAnimation { get; } = new UnityEvent();

        public UnityEvent OnSwipePresentationLeft { get; } = new UnityEvent();

        public UnityEvent OnSwipePresentationRight { get; } = new UnityEvent();

        public UnityEvent OnPlayPausePresentationVideo { get; } = new UnityEvent();

        public UnityEvent OnPresentationVideoReset { get; } = new UnityEvent();

        public UnityEvent OnCameraViewSwitch { get; } = new UnityEvent();

        public UnityEvent OnInteraction { get; } = new UnityEvent();

        public void SwitchSideMenu()
        {
            OnSideMenuSwitch?.Invoke();
        }

        public void PlaySpeakerAnimation()
        {
            OnPlaySpeakerAnimation?.Invoke();
        }

        public void SwipePresentationSlideLeft()
        {
            OnSwipePresentationLeft?.Invoke();
        }

        public void SwipePresentationSlideRight()
        {
            OnSwipePresentationRight?.Invoke();
        }

        public void PlayPausePresentationVideo()
        {
            OnPlayPausePresentationVideo?.Invoke();
        }

        public void ResetPresentationVideo()
        {
            OnPresentationVideoReset?.Invoke();
        }

        public void SwitchCameraView()
        {
            OnCameraViewSwitch?.Invoke();
        }

        public void Interact()
        {
            OnInteraction?.Invoke();
        }
    }
}