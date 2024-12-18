using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Avatars.Player
{    
    [RequireComponent(typeof(PhotonView))]
    public sealed class PlayerUniqueComponents : MonoBehaviour
    {
        [SerializeField] private GameObject _playerControllUniqueComponents;

        [SerializeField] private Rigidbody _playerUniqueRigidbody;
        [SerializeField] private List<Collider> _playerUniqueColliders;

        private PhotonView _photonView;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            CheckIfPlayerIsLocal();
        }

        private void CheckIfPlayerIsLocal()
        {
            if (_photonView.IsMine)
            {
                _playerControllUniqueComponents.SetActive(true);
                return;
            }

            SetupUniqueComponents();
        }

        private void SetupUniqueComponents()
        {
            Destroy(_playerControllUniqueComponents);
            _playerUniqueRigidbody.isKinematic = true;
            _playerUniqueColliders.ForEach(Destroy);
            Destroy(this);
        }
    }
}
