using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core.ServiceLocator;
using LocationsManagement.Interfaces;
using Metaverse.Teleport.Interfaces;

namespace Metaverse.Teleport.TeleportMap
{
    public sealed class MapTeleporter : Teleporter
    {
        [SerializeField] private List<TeleportPoint> _teleportPoints = new List<TeleportPoint>();

        #region Services

        private ILocationLoaderService _locationLoaderInstance;
        private ILocationLoaderService _locationsLoader
            => _locationLoaderInstance ??= Service.Instance.Get<ILocationLoaderService>();

        private IStartupPointHolderService _startupPointHolderInstance;
        private IStartupPointHolderService _startupPointHolder
            => _startupPointHolderInstance ??= Service.Instance.Get<IStartupPointHolderService>();

        private ITeleportControllerService _mapTeleportControllerInstance;
        private ITeleportControllerService _mapTeleportController
            => _mapTeleportControllerInstance ??= Service.Instance.Get<ITeleportControllerService>();

        #endregion

        private void OnEnable()
        {
            _mapTeleportController.TeleportToMapPointEvent.AddListener(TeleportToMapPoint);
        }

        private void OnDisable()
        {
            _mapTeleportController.TeleportToMapPointEvent.RemoveListener(TeleportToMapPoint);
        }

        private void TeleportToMapPoint(TeleportPointType teleportPointType)
        {
            if (teleportPointType.LocationType == _locationsLoader.LoadedLocation.LocationType)
            {
                TeleportPoint teleportPoint = GetPointByMapPointType(teleportPointType.PointType);
                DoTeleport(new Pose(teleportPoint.SelectRandomizedPosition(), teleportPoint.TeleportRotation));
                _mapTeleportController.TeleportWithMap();
                return;
            }

            TeleportToLocation(teleportPointType);
            _mapTeleportController.TeleportWithMap();
        }

        private void TeleportToLocation(TeleportPointType teleportPointType)
        {
            _startupPointHolder.StartupPointType = teleportPointType.PointType;
            _locationsLoader.ChangeLocation(teleportPointType.LocationType);
        }

        private TeleportPoint GetPointByMapPointType(string teleportPointType)
        {
            return _teleportPoints.FirstOrDefault(point => point.TeleportPointType.PointType == teleportPointType);
        }
    }
}