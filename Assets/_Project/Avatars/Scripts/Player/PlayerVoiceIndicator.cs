using UnityEngine;
using UnityEngine.UI;
using VoiceChat.WebGL;
using Photon.Pun;
using System.Linq;
using Core.ServiceLocator;
using ExitGames.Client.Photon;
using PhotonEngine.CustomProperties;
using PhotonEngine.CustomProperties.Enums;

namespace Avatars.Player
{
    public sealed class PlayerVoiceIndicator : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _volumeActive;
        [SerializeField] private GameObject _volumeInactive;

        [SerializeField] private Image _volumeActiveIndicator;

        //The volume is an integer ranging from 0 to 100. Usually, a user with a volume level above 5 is a speaking user.
        private const double MinMicrophoneActiveValue = 20;
        private const double ZeroMicrophoneVolumeValue = 0;

        private PhotonView _photonView;

        private int _currentActorNumber;

        #region Services

        private IPlayerCustomPropertiesService _playerCustomPropertiesService;
        private IPlayerCustomPropertiesService _playerCustomProperties
            => _playerCustomPropertiesService ??= Service.Instance.Get<IPlayerCustomPropertiesService>();

        private IWebVoiceChatAPIService _webVoiceChatAPIService;
        private IWebVoiceChatAPIService _webVoiceChatAPI
            => _webVoiceChatAPIService ??= Service.Instance.Get<IWebVoiceChatAPIService>();

        #endregion

        private void Awake()
        {
            GetPhotonView();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _webVoiceChatAPI.OnPlayerVolumesReceived?.AddListener(PlayersVolumesReceived);
        }

        private void Start()
        {
            _currentActorNumber = _photonView.CreatorActorNr;
            CheckPlayerMicrophoneActiveProperty(_photonView.Owner);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _webVoiceChatAPI.OnPlayerVolumesReceived?.RemoveListener(PlayersVolumesReceived);
        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
            if (targetPlayer.ActorNumber != _photonView.CreatorActorNr)
            {
                return;
            }

            CheckPlayerMicrophoneActiveProperty(targetPlayer);
        }

        private void CheckPlayerMicrophoneActiveProperty(Photon.Realtime.Player targetPlayer)
        {
            if (_playerCustomProperties.IsPlayerHaveCustomProperty(targetPlayer, PlayerCustomPropertyKey.MicrophoneIsActive, out object property))
            {
                bool isMicrophoneActive = System.Convert.ToBoolean(property);

                SetVolumeIndicatorsStates(isMicrophoneActive, !isMicrophoneActive);
            }
        }

        private void PlayersVolumesReceived(PlayersVolume playersVolume)
        {
            PlayerVolumeModel volumeModel = playersVolume.volumes.FirstOrDefault(volumeModel => volumeModel.uid == _currentActorNumber);

            if (volumeModel != null)
            {
                SetActiveIndicatorForVolume(volumeModel.level);
                return;
            }

            SetActiveIndicatorForVolume(ZeroMicrophoneVolumeValue);
        }

        private void SetActiveIndicatorForVolume(double volume)
        {
            _volumeActiveIndicator.gameObject.SetActive(volume > MinMicrophoneActiveValue);
        }

        private void SetVolumeIndicatorsStates(bool volumeActiveState, bool volumeInactiveState)
        {
            _volumeActive.SetActive(volumeActiveState);
            _volumeInactive.SetActive(volumeInactiveState);
        }

        private void GetPhotonView()
        {
            _photonView = GetComponentInParent<PhotonView>();

            if (_photonView == null)
            {
                Debug.LogError("Reference on PhotonView component in parent object is null");
            }
        }
    }
}