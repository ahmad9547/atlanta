using UnityEngine;

namespace Avatars.PersonMovement.MoveSettings
{
    [CreateAssetMenu(fileName = "FlySettings", menuName = "ScriptableObjects/MoveSettings/FlySettings")]
    public sealed class FlySettings : ScriptableObject
    {
        [SerializeField] private float _flyVerticalSpeed = 10f;
        [SerializeField] private float _flyHorizontalSpeed = 15f;

        public float FlyVerticalSpeed => _flyVerticalSpeed;

        public float FlyHorizontalSpeed => _flyHorizontalSpeed;
    }
}
