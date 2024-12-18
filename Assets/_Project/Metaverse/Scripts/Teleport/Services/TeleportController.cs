using System.Collections.Generic;
using UnityEngine.Events;

namespace Metaverse.Teleport.Interfaces
{
    public class TeleportController : ITeleportControllerService
    {
        public UnityEvent<TeleportPointType> TeleportToMapPointEvent { get; set; } =
            new UnityEvent<TeleportPointType>();
        public UnityEvent<TeleportPoint> TeleportToPointEvent { get; set; } = new UnityEvent<TeleportPoint>();
        public UnityEvent TeleportedWithMapEvent { get; set; } = new UnityEvent();

        public ITeleportControllerService.GetTeleportPointsHandler GetTeleportPointsEvent { get; set; }
        public ITeleportControllerService.GetTeleportPointsHandler GetLocationTeleportPointsEvent { get; set; }

        public void TeleportToMapPoint(TeleportPointType teleportPointType)
        {
            TeleportToMapPointEvent?.Invoke(teleportPointType);
        }

        public void TeleportToPoint(TeleportPoint teleportPoint)
        {
            TeleportToPointEvent?.Invoke(teleportPoint);
        }

        public IEnumerable<TeleportPoint> GetCityGatesTeleportPoints()
        {
            return GetTeleportPointsEvent.Invoke();
        }

        public IEnumerable<TeleportPoint> GetLocationTeleportPoints()
        {
            return GetLocationTeleportPointsEvent.Invoke();
        }

        public void TeleportWithMap()
        {
            TeleportedWithMapEvent?.Invoke();
        }
    }
}