using UnityEngine;

namespace Metaverse.SeatingModule
{
    public sealed class StandingUpPosition : MonoBehaviour
    {
        [Header("Debug:")]
        [SerializeField] private Color _seatingPlaceColor;
        [SerializeField] private Vector3 _seatingPlaceSize;

        public Vector3 Position => transform.position;

        private void OnDrawGizmos()
        {
            Gizmos.color = _seatingPlaceColor;
            Gizmos.DrawCube(transform.position, _seatingPlaceSize);
        }
    }
}
