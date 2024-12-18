using Core.ServiceLocator;
using Core.UI;
using DG.Tweening;
using Metaverse.GiftShop.Interfaces;
using Metaverse.InteractionModule;
using Metaverse.InteractionModule.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Metaverse.GiftShop.MusicPlayer
{
    public sealed class MusicPlayer : MonoBehaviour, IInteractionZoneTrigger, IInteractionZoneButton
    {
        private const float MutedValue = 0f;
        private const float UnmutedValue = 0.1f;
        private const float Duration = 1f;

        [SerializeField] private InteractionZoneHandler _interactionZoneHandler;
        [SerializeField] private AudioSource _backgroundAudioSource;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Image _itemInfoImage;

        private UIAnimator _uiAnimator = new UIAnimator();

        #region Services

        private IMusicPlayerEventHandler _musicPlayerEventHandlerInstance;
        private IMusicPlayerEventHandler _musicPlayerEventHandler
            => _musicPlayerEventHandlerInstance ??= Service.Instance.Get<IMusicPlayerEventHandler>();

        #endregion

        private void OnEnable()
        {
            _interactionZoneHandler.AddInteractionZoneButtonObserver(this);
            _interactionZoneHandler.AddInteractionZoneTriggerObserver(this);
            _musicPlayerEventHandler.OnMusicClipSelected.AddListener(OnMusicClipSelected);
        }

        private void OnDisable()
        {
            _interactionZoneHandler.RemoveInteractionZoneButtonObserver(this);
            _interactionZoneHandler.RemoveInteractionZoneTriggerObserver(this);
            _musicPlayerEventHandler.OnMusicClipSelected.RemoveListener(OnMusicClipSelected);
        }

        public void OnInteractionZoneTriggerEnter(GameObject player)
        {
            MuteBackgroundNoise();
            _itemInfoImage.gameObject.SetActive(true);
            _uiAnimator.ShowWindow(_itemInfoImage.transform);
        }

        public void OnInteractionZoneTriggerExit(GameObject player)
        {
            UnmuteBackgroundNoise();
            _musicPlayerEventHandler.HideMusicPlayer();

            _uiAnimator.HideWindow(_itemInfoImage.transform, () =>
            {
                _itemInfoImage.gameObject.SetActive(false);
            });
        }

        public void StartInteractionButtonClick()
        {
            _musicPlayerEventHandler.ShowMusicPlayer();
        }

        public void EndInteractionButtonClick()
        {
        }

        private void OnMusicClipSelected(int number)
        {
            _audioSource.clip = _musicPlayerEventHandler.MusicClips[number].Clip;
            _audioSource.Play();
        }

        private void UnmuteBackgroundNoise()
        {
            ChangeVolume(UnmutedValue);
        }

        private void MuteBackgroundNoise()
        {
            ChangeVolume(MutedValue);
        }

        private void ChangeVolume(float endValue)
        {
            DOTween.To(() => _backgroundAudioSource.volume, value => _backgroundAudioSource.volume = value, endValue,
                Duration);
        }
    }
}