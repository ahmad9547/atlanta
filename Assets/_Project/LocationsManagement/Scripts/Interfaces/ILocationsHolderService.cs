using Core.ServiceLocator;
using LocationsManagement.Enums;
using System.Collections.Generic;

namespace LocationsManagement.Interfaces
{
    public interface ILocationsHolderService : IService
    {
        void SetLocations(List<Location> locations);

        Location GetLocationByType(LocationType locationType);

        Location GetFirstLocation();
    }
}