using Avatars.WebGLMovement;
using Avatars.PersonMovement;
using UnityEngine;

namespace Avatars.Player
{
    public sealed class PlayerComponentsReferences : MonoBehaviour
    {
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerMovement _playerMovement;

        public PlayerAnimator PlayerAnimator { get { return _playerAnimator; } }
        public PlayerMovement PlayerMovement { get { return _playerMovement; } }
    }
}
