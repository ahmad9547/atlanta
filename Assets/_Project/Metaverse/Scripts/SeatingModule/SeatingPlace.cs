using UnityEngine;

namespace Metaverse.SeatingModule
{
    public sealed class SeatingPlace : MonoBehaviour
    {
        [SerializeField] private StandingUpPosition _standingUpPosition;

        [Header("Debug:")]
        [SerializeField] private Color _seatingPlaceColor;
        [SerializeField] private Vector3 _seatingPlaceSize;

        public int Id { get; private set; }

        public bool IsFree { get; set; } = true;

        public Vector3 Position => transform.position;

        public Vector3 StandingUpPosition => _standingUpPosition.Position;

        private MetaverseSeatingPlaces _metaverseSeatingPlaces;

        public void Initialize(MetaverseSeatingPlaces metaverseSeatingPlaces, int id)
        {
            _metaverseSeatingPlaces = metaverseSeatingPlaces;
            Id = id;
        }

        public void OccupyPlace()
        {
            IsFree = false;
            _metaverseSeatingPlaces.SetSeatingPlaceOccupied(this);
        }

        public void UnoccupyPlace()
        {
            IsFree = true;
            _metaverseSeatingPlaces.SetSeatingPlaceUnoccupied(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _seatingPlaceColor;
            Gizmos.DrawCube(transform.position, _seatingPlaceSize);
        }
    }
}
