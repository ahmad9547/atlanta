using UnityEngine;
using Core.UI;
using UnityEngine.UI;
using VoiceChat;
using UserManagement;
using Core.ServiceLocator;

namespace PlayerUIScene.SideMenu.Controllers
{
    public sealed class AdminTab : UIController
    {
        private const float DisabledGlobalVoiceSoundValue = 0f;

        [Header("Global voice channel button")]
        [SerializeField] private Button _globalVoiceOffButton;
        [SerializeField] private Button _globalVoiceOnButton;

        [Header("Global voice slider settings")]
        [SerializeField] private CanvasGroup _globalVoiceSliderCanvasGroup;
        [SerializeField] private Slider _globalVoiceSlider;
        [SerializeField] private Button _soundOffButton;
        [SerializeField] private Button _soundOnButton;

        [SerializeField] private MenuOptionButton _adminAreaOptionButton;

        private float _globalVoiceSoundLevel = 0.25f;

        #region Services

        private IUserProfileService _userProfileService;
        private IUserProfileService _userProfile
            => _userProfileService ??= Service.Instance.Get<IUserProfileService>();

        private IVoiceChatService _voiceChatService;
        private IVoiceChatService _voiceChat
            => _voiceChatService ??= Service.Instance.Get<IVoiceChatService>();

        #endregion

        private void OnEnable()
        {
            _globalVoiceOffButton.onClick.AddListener(SetGlobalVoiceOff);
            _globalVoiceOnButton.onClick.AddListener(SetGlobalVoiceOn);

            _soundOffButton.onClick.AddListener(SoundOfGlobalVoiceOff);
            _soundOnButton.onClick.AddListener(SoundOfGlobalVoiceOn);

            _globalVoiceSlider.onValueChanged.AddListener(GlobalVoiceVolumeChanged);
        }

        protected override void Start()
        {
            CheckAdminStatus();
            SetGlobalVoiceOff();
        }

        private void OnDisable()
        {
            _globalVoiceOffButton.onClick.RemoveListener(SetGlobalVoiceOff);
            _globalVoiceOnButton.onClick.RemoveListener(SetGlobalVoiceOn);

            _soundOffButton.onClick.RemoveListener(SoundOfGlobalVoiceOff);
            _soundOnButton.onClick.RemoveListener(SoundOfGlobalVoiceOn);

            _globalVoiceSlider.onValueChanged.RemoveListener(GlobalVoiceVolumeChanged);
        }

        private void SetGlobalVoiceOff()
        {
            _uiAnimator.ButtonScale(_globalVoiceOffButton, () =>
            {
                _voiceChat.MuteInAdminChannel();
                ToggleGlobalVoiceButton(true);
                SoundOfGlobalVoiceOff();
                ToggleGlobalVoiceSliderCanvasGroup(false);
            });
        }

        private void SetGlobalVoiceOn()
        {
            _uiAnimator.ButtonScale(_globalVoiceOnButton, () =>
            {
                _voiceChat.UnmuteInAdminChannel();
                ToggleGlobalVoiceButton(false);
                SoundOfGlobalVoiceOn();
                ToggleGlobalVoiceSliderCanvasGroup(true);
            });
        }

        private void CheckAdminStatus()
        {
            _adminAreaOptionButton.gameObject.SetActive(_userProfile.IsAdmin);
        }

        private void ToggleGlobalVoiceButton(bool isVoiceOnButtonVisible)
        {
            _globalVoiceOffButton.gameObject.SetActive(!isVoiceOnButtonVisible);
            _globalVoiceOnButton.gameObject.SetActive(isVoiceOnButtonVisible);
        }

        private void SoundOfGlobalVoiceOn()
        {
            SetGlobalVoiceSliderStateWithValue(true, _globalVoiceSoundLevel);
            SetSoundButtonActiveState(true);
            _voiceChat.SetAdminVoiceLocalVolume(_globalVoiceSoundLevel);
        }

        private void SoundOfGlobalVoiceOff()
        {
            SetGlobalVoiceSliderStateWithValue(false, DisabledGlobalVoiceSoundValue);
            SetSoundButtonActiveState(false);
            _voiceChat.SetAdminVoiceLocalVolume(DisabledGlobalVoiceSoundValue);
        }

        private void GlobalVoiceVolumeChanged(float value)
        {
            _globalVoiceSoundLevel = value;
            _voiceChat.SetAdminVoiceLocalVolume(value);
        }

        private void SetSoundButtonActiveState(bool soundIsActive)
        {
            _soundOnButton.gameObject.SetActive(!soundIsActive);
            _soundOffButton.gameObject.SetActive(soundIsActive);
        }

        private void SetGlobalVoiceSliderStateWithValue(bool isSliderActive, float value)
        {
            _globalVoiceSlider.SetValueWithoutNotify(value);
            _globalVoiceSlider.interactable = isSliderActive;
        }

        private void ToggleGlobalVoiceSliderCanvasGroup(bool isActive)
        {
            _globalVoiceSliderCanvasGroup.interactable = isActive;
            _globalVoiceSliderCanvasGroup.blocksRaycasts = isActive;
        }
    }
}