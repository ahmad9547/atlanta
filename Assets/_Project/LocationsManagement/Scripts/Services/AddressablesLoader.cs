using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.AddressableAssets.ResourceLocators;
using LocationsManagement.Interfaces;
using ProjectConfig.General;

namespace LocationsManagement.Services
{
    public sealed class AddressablesLoader : IAddressablesLoaderService
    {
        // This path is used in AWSProfile addressables profile as Remote.LoadPath field with
        // {LocationsManagement.Services.AddressablesLoaderService.RemoteAddressablesPath} parameter
        public static string RemoteAddressablesPath = string.Empty;

        public async Task Initialize()
        {
            RemoteAddressablesPath = GeneralSettings.AddressablesBundlesUrl;

            AsyncOperationHandle<IResourceLocator> handle = Addressables.InitializeAsync();

            await handle.Task;
        }

        public async Task<SceneInstance> LoadSceneFromAssetReference(AssetReference sceneAssetReference,
            LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneAssetReference, loadSceneMode);

            SceneInstance sceneInstance = await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return sceneInstance;
            }
            else
            {
                Debug.LogError("Failed while loading SceneInstance from addressables bundle");
                return default;
            }
        }

        public async Task UnloadScene(SceneInstance sceneInstance)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.UnloadSceneAsync(sceneInstance);

            await handle.Task;
        }
    }
}
