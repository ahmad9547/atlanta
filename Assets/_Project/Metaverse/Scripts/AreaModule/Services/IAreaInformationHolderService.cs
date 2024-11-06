using Core.ServiceLocator;
using UnityEngine.Events;

namespace Metaverse.AreaModule.Services
{
    public interface IAreaInformationHolderService : IService
    {
        AreaInformation CurrentAreaInformation { get; }

        UnityEvent<AreaInformation> OnPlayerAreaChanged { get; }

        void ChangePlayerArea(AreaInformation areaInformation);
    }
}