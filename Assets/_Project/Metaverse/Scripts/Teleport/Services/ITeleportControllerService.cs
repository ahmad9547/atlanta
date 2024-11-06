using System.Collections.Generic;
using Core.ServiceLocator;
using UnityEngine.Events;

namespace Metaverse.Teleport.Interfaces
{
    public interface ITeleportControllerService : IService
    {
        UnityEvent<TeleportPointType> TeleportToMapPointEvent { get; set; }
        UnityEvent<TeleportPoint> TeleportToPointEvent { get; set; }
        UnityEvent TeleportedWithMapEvent { get; set; }

        delegate IEnumerable<TeleportPoint> GetTeleportPointsHandler();
        GetTeleportPointsHandler GetTeleportPointsEvent { get; set; }
        GetTeleportPointsHandler GetLocationTeleportPointsEvent { get; set; }

        void TeleportToMapPoint(TeleportPointType teleportPointType);
        void TeleportToPoint(TeleportPoint teleportPoint);
        IEnumerable<TeleportPoint> GetCityGatesTeleportPoints();
        IEnumerable<TeleportPoint> GetLocationTeleportPoints();
        void TeleportWithMap();
    }
}