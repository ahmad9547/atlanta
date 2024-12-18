using Metaverse.Teleport.Enums;
using UnityEngine;

namespace Metaverse.Teleport.Database
{
    [System.Serializable]
    public sealed class TeleportPointName
    {
        [SerializeField] private string _pointType;
        [SerializeField] private string _pointName;

        public string PointType { get { return _pointType; } }
        public string PointName { get { return _pointName; } }
    }
}
