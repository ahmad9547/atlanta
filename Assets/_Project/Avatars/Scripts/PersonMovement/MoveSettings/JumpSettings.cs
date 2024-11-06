using UnityEngine;

namespace Avatars.PersonMovement.MoveSettings
{
    [CreateAssetMenu(fileName = "JumpSettings", menuName = "ScriptableObjects/MoveSettings/JumpSettings")]
    public sealed class JumpSettings : ScriptableObject
    {
        [SerializeField] private float _jumpHeight = 1.3f;

        public float JumpHeight => _jumpHeight;
    }
}
