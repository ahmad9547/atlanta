using Core.ServiceLocator;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LocationsManagement.Interfaces
{
    public interface IAddressablesLoaderService : IService
    {
        Task Initialize();

        Task<SceneInstance> LoadSceneFromAssetReference(AssetReference sceneAssetReference,
            LoadSceneMode loadSceneMode = LoadSceneMode.Additive);

        Task UnloadScene(SceneInstance sceneInstance);
    }
}