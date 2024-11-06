using System.Collections.Generic;
using Core.ServiceLocator;
using Metaverse.Services;
using UnityEngine;

namespace VoiceChat.Player
{
    public class SpatialVoiceManager : ISpatialVoiceManagerService
    {
        private const float SpatialVoiceMaxDistance = 12f;
        private const float SpatialVoiceCheckInterval = 0.5f;
        private const float SpatialVoiceMinDistance = 4f;
        private const float VoiceVolumeLevelMultiplier = 100f;
        private const int VoiceVolumeLevelCoefficient = 1;

        private readonly Dictionary<int, PlayerSpatialVoice> _spatialVoicePlayerIds = new Dictionary<int, PlayerSpatialVoice>();

        private float _spatialVoiceCheckTimer;

        #region Services

        private ILocalPlayerService _localPlayerHolderInstance;
        private ILocalPlayerService _localPlayerHolder
            => _localPlayerHolderInstance ??= Service.Instance.Get<ILocalPlayerService>();

        private IVoiceChatService _voiceChatService;
        private IVoiceChatService _voiceChat
            => _voiceChatService ??= Service.Instance.Get<IVoiceChatService>();

        #endregion

        private Transform _localPlayerInstance;
        private Transform _localPlayer
            => _localPlayerInstance ??= _localPlayerHolder.LocalPlayer.transform;

        public float GetSpatialVoiceMaxDistance()
        {
            return SpatialVoiceMaxDistance;
        }

        public void RegisterSpatialVoicePlayer(int playerId, PlayerSpatialVoice playerSpatialVoice)
        {
            if (_spatialVoicePlayerIds.ContainsKey(playerId))
            {
                Debug.LogError($"SpatialVoice component for {playerId} player id already registered");
                return;
            }

            _spatialVoicePlayerIds.Add(playerId, playerSpatialVoice);
        }

        public void UnregisterSpatialVoicePlayer(int playerId)
        {
            if (!_spatialVoicePlayerIds.ContainsKey(playerId))
            {
                Debug.LogError($"SpatialVoice with {playerId} player id is not registered");
                return;
            }

            _spatialVoicePlayerIds.Remove(playerId);
        }

        public void UpdateSpatialVoiceCheckTimer()
        {
            _spatialVoiceCheckTimer += Time.deltaTime;

            if (_spatialVoiceCheckTimer >= SpatialVoiceCheckInterval)
            {
                _spatialVoiceCheckTimer = 0f;
                CheckPlayersDistance();
            }
        }

        private void CheckPlayersDistance()
        {
            foreach ((int key, PlayerSpatialVoice playerSpatialVoice) in _spatialVoicePlayerIds)
            {
                if (playerSpatialVoice == null)
                {
                    continue;
                }

                float distance = Vector3.Distance(playerSpatialVoice.Position, _localPlayer.position);

                _voiceChat.SetPlayerVolumeLevel(key, GetVolumeLevelFromDistance(distance));
            }
        }

        private float GetVolumeLevelFromDistance(float distance)
        {
            if (distance >= SpatialVoiceMaxDistance)
            {
                return 0;
            }

            float relativeDistance =
                Mathf.Clamp01((distance - SpatialVoiceMinDistance) / (SpatialVoiceMaxDistance - SpatialVoiceMinDistance));
            return Mathf.Abs(relativeDistance - VoiceVolumeLevelCoefficient) * VoiceVolumeLevelMultiplier;
        }
    }
}