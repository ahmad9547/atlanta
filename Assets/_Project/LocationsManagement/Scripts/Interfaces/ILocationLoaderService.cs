using Core.ServiceLocator;
using LocationsManagement.Enums;

namespace LocationsManagement.Interfaces
{
    public interface ILocationLoaderService : IService
    {
        Location LoadedLocation { get; }

        void LoadFirstLocation();

        void ChangeLocation(LocationType nextLocationType);
    }
}