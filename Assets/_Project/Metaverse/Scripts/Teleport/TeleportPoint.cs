using UnityEngine;

namespace Metaverse.Teleport
{
    public sealed class TeleportPoint : MonoBehaviour
    {
        private const int Divider = 2;
        
        [SerializeField] private TeleportPointType _teleportPointType;
        [SerializeField] private Quaternion _teleportRotation;
        [SerializeField] private Vector3 _teleportZoneSize = Vector3.one;

        [Header("Debug:")] [SerializeField] private Color _teleportPointColor;

        public TeleportPointType TeleportPointType => _teleportPointType;
        public Vector3 Position => transform.position;
        public Quaternion TeleportRotation => _teleportRotation;

        public Vector3 SelectRandomizedPosition()
        {
            float xPosition = Random.Range(transform.position.x - (_teleportZoneSize.x / Divider), transform.position.x + (_teleportZoneSize.x / Divider));
            float yPosition = Random.Range(transform.position.y - (_teleportZoneSize.y / Divider), transform.position.y + (_teleportZoneSize.y / Divider));
            float zPosition = Random.Range(transform.position.z - (_teleportZoneSize.z / Divider), transform.position.z + (_teleportZoneSize.z / Divider));

            return new Vector3(xPosition, yPosition, zPosition);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _teleportPointColor;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}