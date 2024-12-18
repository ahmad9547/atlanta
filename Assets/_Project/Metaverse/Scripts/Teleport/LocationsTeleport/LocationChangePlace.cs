using Core.ServiceLocator;
using LocationsManagement.Interfaces;
using Metaverse.Services;
using Metaverse.Teleport.CityGates;
using Metaverse.Teleport.Interfaces;
using UnityEngine;

namespace Metaverse.Teleport.LocationsTeleport
{
    public class LocationChangePlace : MonoBehaviour
    {
        [SerializeField] protected CityGateTeleportTriggerZone _triggerZone;
        [SerializeField] protected TeleportPointType _destinationPointType;

        #region Services

        private ILocalPlayerService _localPlayerHolderInstance;
        private ILocalPlayerService _localPlayerHolder
            => _localPlayerHolderInstance ??= Service.Instance.Get<ILocalPlayerService>();

        private ILocationLoaderService _locationLoaderInstance;
        private ILocationLoaderService _locationsLoader
            => _locationLoaderInstance ??= Service.Instance.Get<ILocationLoaderService>();

        private IStartupPointHolderService _startupPointHolderInstance;
        private IStartupPointHolderService _startupPointHolder
            => _startupPointHolderInstance ??= Service.Instance.Get<IStartupPointHolderService>();

        #endregion

        protected virtual void OnEnable()
        {
            _triggerZone.OnEnter.AddListener(OnTriggerZoneEnter);
        }

        protected virtual void OnDisable()
        {
            _triggerZone.OnEnter.RemoveListener(OnTriggerZoneEnter);
        }

        private void OnTriggerZoneEnter(GameObject player)
        {
            if (_localPlayerHolder.IsOtherPlayerEqualsLocalPlayer(player))
            {
                ChangeLocation();
            }
        }

        private void ChangeLocation()
        {
            _startupPointHolder.StartupPointType = _destinationPointType.PointType;
            _locationsLoader.ChangeLocation(_destinationPointType.LocationType);
        }
    }
}