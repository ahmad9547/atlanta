using UnityEngine.Events;

namespace Metaverse.AreaModule.Services
{
    public sealed class AreaInformationHolder : IAreaInformationHolderService
    {
        public AreaInformation CurrentAreaInformation { get; private set; }

        public UnityEvent<AreaInformation> OnPlayerAreaChanged { get; } = new();

        public void ChangePlayerArea(AreaInformation areaInformation)
        {
            CurrentAreaInformation = areaInformation;
            OnPlayerAreaChanged?.Invoke(areaInformation);
        }
    }
}