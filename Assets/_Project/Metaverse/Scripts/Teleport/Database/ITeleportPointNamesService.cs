using Core.ServiceLocator;

namespace Metaverse.Teleport.Database
{
    public interface ITeleportPointNamesService : IService
    {
        void InitializeTeleportPointNames(TeleportPointNamesDatabase teleportPointNamesDatabase);
        string GetPointNameByType(string pointType);
    }
}