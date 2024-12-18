using UnityEngine;

namespace Avatars.PersonMovement.MoveSettings
{
    [CreateAssetMenu(fileName = "WalkSettings", menuName = "ScriptableObjects/MoveSettings/WalkSettings")]
    public sealed class WalkSettings : ScriptableObject
    {
        [SerializeField] private float _walkSpeed = 3.5f;

        public float WalkSpeed => _walkSpeed;
    }
}
