using Core.ServiceLocator;

namespace Metaverse.Teleport.Interfaces
{
    public interface IStartupPointHolderService : IService
    {
        string StartupPointType { get; set; }
    }
}