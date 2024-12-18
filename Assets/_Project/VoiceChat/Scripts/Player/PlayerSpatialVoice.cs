using Core.ServiceLocator;
using Photon.Pun;
using UnityEngine;

namespace VoiceChat.Player
{
    [RequireComponent(typeof(PhotonView))]
    public sealed class PlayerSpatialVoice : MonoBehaviour
    {
        public Vector3 Position => transform.position;

        private PhotonView _photonView;
        private bool _isLocalPlayer;

        #region Services

        private ISpatialVoiceManagerService _spatialVoiceManagerInstance;
        private ISpatialVoiceManagerService _spatialVoiceManager
            => _spatialVoiceManagerInstance ??= Service.Instance.Get<ISpatialVoiceManagerService>();

        #endregion

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _isLocalPlayer = _photonView.IsMine;
        }

        private void Start()
        {
            RegisterCurrentPlayerSpatialVoice();
        }

        private void OnDestroy()
        {
            UnregisterCurrentPlayerSpatialVoice();
        }

        private void OnDrawGizmos()
        {
            if (_spatialVoiceManager == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _spatialVoiceManager.GetSpatialVoiceMaxDistance());
        }

        private void RegisterCurrentPlayerSpatialVoice()
        {
            if (_isLocalPlayer)
            {
                return;
            }

            _spatialVoiceManager.RegisterSpatialVoicePlayer(_photonView.OwnerActorNr, this);
        }

        private void UnregisterCurrentPlayerSpatialVoice()
        {
            if (_isLocalPlayer)
            {
                return;
            }

            _spatialVoiceManager.UnregisterSpatialVoicePlayer(_photonView.OwnerActorNr);
        }
    }
}