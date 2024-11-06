using Core.ServiceLocator;

namespace StartScene.Services
{
    public interface IStartSceneStateHolderService : IService
    {
        bool WasAlreadyLoaded { get; set; }
    }
}