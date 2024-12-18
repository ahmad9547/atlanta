using Avatars.PersonMovement.MoveSettings;
using Core.Helpers;
using LocationsManagement.Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace LocationsManagement
{
    [CreateAssetMenu(fileName = "Location", menuName = "ScriptableObjects/Location")]
    public sealed class Location : ScriptableObject
    {
        [SerializeField] private LocationType _locationType;
        [SerializeField] private AssetReference _locationReference;
        [SerializeField] private GameObject _locationFunctionalityPrefab;
        [SerializeField] private MoveSettingHolder _locationMoveSettings;

        public LocationType LocationType => _locationType;
        public AssetReference LocationReference => _locationReference;
        public GameObject LocationFunctionalityPrefab => _locationFunctionalityPrefab;
        public MoveSettingHolder LocationMoveSettings => _locationMoveSettings;

        public string LocationName => StringHelpers.GetSpaceSplitedString(_locationType.ToString());

        public SceneInstance LocationSceneInstance { get; set; }

        public void CleanAssetReferences()
        {
            LocationSceneInstance = default;
        }
    }
}
