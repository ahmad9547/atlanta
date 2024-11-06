using UnityEngine;

namespace Avatars.AvatarsPhysics
{
    public sealed class PlayerStandOn : MonoBehaviour
    {
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private float _impulseForce = 50f;

        private const string PlayerTag = "PlayersDistance";

        private void OnTriggerEnter(Collider other)
        {
            MovePlayerFromOtherPlayer(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            MovePlayerFromOtherPlayer(other.gameObject);
        }

        private void MovePlayerFromOtherPlayer(GameObject otherPlayer)
        {
            if (!otherPlayer.CompareTag(PlayerTag))
            {
                return;
            }

            Vector3 impulseDirection = ((_playerRigidbody.transform.right * _impulseForce) + _playerRigidbody.transform.up).normalized;
            _playerRigidbody.AddRelativeForce(impulseDirection * _impulseForce, ForceMode.Impulse);
        }
    }
}
