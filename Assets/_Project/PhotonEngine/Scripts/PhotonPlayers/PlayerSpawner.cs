using Photon.Pun;
using UnityEngine;
using Core.ServiceLocator;
using Metaverse.Teleport;

namespace PhotonEngine.PhotonPlayers
{
    public sealed class PlayerSpawner : MonoBehaviour
    {
        private const string AvatarPrefabName = "Avatar";

        [SerializeField] private TeleportPointsHolder _teleportPointsHolder;

        #region Services

        private IPlayerCreatorService _playerCreatorInstance;
        private IPlayerCreatorService _playerCreator
            => _playerCreatorInstance ??= Service.Instance.Get<IPlayerCreatorService>();

        #endregion

        private void OnEnable()
        {
            _playerCreator.CreatePlayerEvent += CreatePlayer;
        }

        private void OnDisable()
        {
            _playerCreator.CreatePlayerEvent -= CreatePlayer;
        }

        private GameObject CreatePlayer()
        {
            TeleportPoint targetTeleportPoint = _teleportPointsHolder.SelectTeleportPoint();

            Vector3 spawnPosition = targetTeleportPoint.SelectRandomizedPosition();
            Quaternion spawnRotation = targetTeleportPoint.TeleportRotation;

            GameObject player = PhotonNetwork.Instantiate(AvatarPrefabName, spawnPosition, spawnRotation);

            return player;
        }
    }
}