namespace StartScene.Services
{
    public sealed class StartSceneStateHolder : IStartSceneStateHolderService
    {
        public bool WasAlreadyLoaded { get; set; }
    }
}
