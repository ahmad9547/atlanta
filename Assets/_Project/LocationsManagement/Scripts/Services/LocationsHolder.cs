using System;
using LocationsManagement.Enums;
using LocationsManagement.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Core.ServiceLocator;
using Metaverse.ErrorHandling;
using Metaverse.ErrorHandling.Services;
using ProjectConfig.Session;
using ProjectConfig.Session.Services;

namespace LocationsManagement.Services
{
    public sealed class LocationsHolder : ILocationsHolderService
    {
        private List<Location> _locations = new();

        #region Services

        private static ISessionConfigHandler _sessionConfigHandlerService;
        private static ISessionConfigHandler _sessionConfigHandler
            => _sessionConfigHandlerService ??= Service.Instance.Get<ISessionConfigHandler>();

        #endregion

        public void SetLocations(List<Location> locations)
        {
            _locations = locations;
        }

        public Location GetLocationByType(LocationType locationType)
        {
            return _locations.FirstOrDefault(location => location.LocationType == locationType);
        }

        public Location GetFirstLocation()
        {
            if (Enum.TryParse(SessionConfig.LocationId, out LocationType locationType))
            {
                return GetLocationByType(locationType);
            }

            _sessionConfigHandler.AddErrorMessage($"Unable to find location with {SessionConfig.LocationId} ID. Loading default location");
            return _locations.FirstOrDefault();
        }
    }
}