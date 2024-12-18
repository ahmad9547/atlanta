using System.Collections.Generic;
using System.Linq;

namespace Metaverse.Teleport.Database
{
    public class TeleportPointNames : ITeleportPointNamesService
    {
        private Dictionary<string, string> _teleportPointNames = new Dictionary<string, string>();

        public void InitializeTeleportPointNames(TeleportPointNamesDatabase teleportPointNamesDatabase)
        {
            _teleportPointNames = teleportPointNamesDatabase.TeleportPointNames
                .ToDictionary(pointType => pointType.PointType, pointName => pointName.PointName);
        }

        public string GetPointNameByType(string pointType)
        {
            return _teleportPointNames[pointType];
        }

    }
}