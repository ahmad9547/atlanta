using System.Collections.Generic;
using Core.ServiceLocator;
using Metaverse.Teleport.Interfaces;
using UnityEngine;

namespace Metaverse.Teleport.CityGates
{
    public sealed class CityGatesTeleporter : Teleporter
    {
        [SerializeField] private List<TeleportPoint> _cityGatesTeleportPoints = new List<TeleportPoint>();
        [SerializeField] private List<TeleportPoint> _locationTeleportPoints = new List<TeleportPoint>();

        #region Services

        private ITeleportControllerService _cityGatesTeleportControllerInstance;
        private ITeleportControllerService _cityGatesTeleportController
            => _cityGatesTeleportControllerInstance ??= Service.Instance.Get<ITeleportControllerService>();

        #endregion

        private void OnEnable()
        {
            _cityGatesTeleportController.TeleportToPointEvent.AddListener(TeleportToPoint);
            _cityGatesTeleportController.GetTeleportPointsEvent += GetCityGatesTeleportPoints;
            _cityGatesTeleportController.GetLocationTeleportPointsEvent += GetLocationTeleportPoints;
        }

        private void OnDisable()
        {
            _cityGatesTeleportController.TeleportToPointEvent.RemoveListener(TeleportToPoint);
            _cityGatesTeleportController.GetTeleportPointsEvent -= GetCityGatesTeleportPoints;
            _cityGatesTeleportController.GetLocationTeleportPointsEvent -= GetLocationTeleportPoints;
        }

        private void TeleportToPoint(TeleportPoint teleportPoint)
        {
            if (teleportPoint == null)
            {
                Debug.LogError("Target teleport point reference is null");
                return;
            }

            DoTeleport(new Pose(teleportPoint.Position, teleportPoint.TeleportRotation));
        }

        private IEnumerable<TeleportPoint> GetCityGatesTeleportPoints()
        {
            return _cityGatesTeleportPoints;
        }

        private IEnumerable<TeleportPoint> GetLocationTeleportPoints()
        {
            return _locationTeleportPoints;
        }
    }
}