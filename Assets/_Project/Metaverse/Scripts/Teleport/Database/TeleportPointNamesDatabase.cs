using System.Collections.Generic;
using UnityEngine;

namespace Metaverse.Teleport.Database
{
    [CreateAssetMenu(fileName = "TelepotPointNamesDatabase", menuName = "ScriptableObjects/TeleportPointNamesDatabase")]
    public sealed class TeleportPointNamesDatabase : ScriptableObject
    {
        [SerializeField] private List<TeleportPointName> _teleportPointNames;

        public IEnumerable<TeleportPointName> TeleportPointNames { get { return _teleportPointNames; } }
    }
}
