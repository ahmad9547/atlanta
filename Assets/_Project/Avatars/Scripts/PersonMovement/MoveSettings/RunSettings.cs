using UnityEngine;

namespace Avatars.PersonMovement.MoveSettings
{
    [CreateAssetMenu(fileName = "RunSettings", menuName = "ScriptableObjects/MoveSettings/RunSettings")]
    public sealed class RunSettings : ScriptableObject
    {
        [SerializeField] private float _runSpeed = 6f;

        public float RunSpeed => _runSpeed;
    }
}
