using UnityEngine;
using Core.UI;
using UnityEngine.UI;
using Core.ServiceLocator;
using VoiceChat;

namespace PlayerUIScene.SideMenu.Controllers
{
    public sealed class SoundLevelState : UIController
    {
        private const float DisabledSoundValue = 0f;

        [SerializeField] private Button _soundOffButton;
        [SerializeField] private Button _soundOnButton;

        [SerializeField] private Slider _environmentSoundSlider;
        [SerializeField] private Slider _voiceChatSoundSlider;

        private float _environmentSoundLevel = 1f;
        private float _voiceChatSoundLevel = 1f;

        #region Services

        private IVoiceChatService _voiceChatService;
        private IVoiceChatService _voiceChat
            => _voiceChatService ??= Service.Instance.Get<IVoiceChatService>();

        #endregion

        private void OnEnable()
        {
            _soundOffButton.onClick.AddListener(SoundOff);
            _soundOnButton.onClick.AddListener(SoundOn);

            _environmentSoundSlider.onValueChanged.AddListener(EnvironmentSoundValueChanged);
            _voiceChatSoundSlider.onValueChanged.AddListener(VoiceChatSoundValueChanged);
        }

        protected override void Start()
        {
            Show();
            SoundOn();
        }

        private void OnDisable()
        {
            _soundOffButton.onClick.RemoveListener(SoundOff);
            _soundOnButton.onClick.RemoveListener(SoundOn);

            _environmentSoundSlider.onValueChanged.RemoveListener(EnvironmentSoundValueChanged);
            _voiceChatSoundSlider.onValueChanged.RemoveListener(VoiceChatSoundValueChanged);
        }

        private void SoundOn()
        {
            SetSliderStateWithValue(_environmentSoundSlider, true, _environmentSoundLevel);
            SetSliderStateWithValue(_voiceChatSoundSlider, true, _voiceChatSoundLevel);
            SetSoundButtonActiveState(true);
            AudioListener.volume = _environmentSoundLevel;
            _voiceChat.SetVoiceChatSoundLevel(_voiceChatSoundLevel);
        }

        private void SoundOff()
        {
            SetSliderStateWithValue(_environmentSoundSlider, false, DisabledSoundValue);
            SetSliderStateWithValue(_voiceChatSoundSlider, false, DisabledSoundValue);
            SetSoundButtonActiveState(false);
            AudioListener.volume = DisabledSoundValue;
            _voiceChat.SetVoiceChatSoundLevel(DisabledSoundValue);
        }

        private void EnvironmentSoundValueChanged(float value)
        {
            _environmentSoundLevel = value;
            AudioListener.volume = value;
        }

        private void VoiceChatSoundValueChanged(float value)
        {
            _voiceChatSoundLevel = value;
            _voiceChat.SetVoiceChatSoundLevel(value);
        }

        private void SetSoundButtonActiveState(bool soundIsActive)
        {
            _soundOnButton.gameObject.SetActive(!soundIsActive);
            _soundOffButton.gameObject.SetActive(soundIsActive);
        }

        private void SetSliderStateWithValue(Slider slider, bool isSliderActive, float value)
        {
            slider.SetValueWithoutNotify(value);
            slider.interactable = isSliderActive;
        }
    }
}