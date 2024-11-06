using System;
using Core.ServiceLocator;
using Metaverse.PresetModification.Interfaces;
using Metaverse.Services;
using PhotonEngine.CustomProperties.Services;
using UnityEngine;

namespace Metaverse.PresetModification
{
    public sealed class PresetModificationArea : MonoBehaviour
    {
        private const int DefaultPresetNumber = 0;

        [SerializeField] private RoomType _roomType;
        [SerializeField] private PresetsModificationAsset _presetsModificationAsset;

        #region Services

        private ILocalPlayerService _localPlayerHolderInstance;
        private ILocalPlayerService _localPlayerHolder
            => _localPlayerHolderInstance ??= Service.Instance.Get<ILocalPlayerService>();

        private IRoomCustomPropertiesService _roomCustomPropertiesService;
        private IRoomCustomPropertiesService _roomCustomProperties
            => _roomCustomPropertiesService ??= Service.Instance.Get<IRoomCustomPropertiesService>();

        private IPresetModificationService _presetModificatorInstance;
        private IPresetModificationService _presetModificator
            => _presetModificatorInstance ??= Service.Instance.Get<IPresetModificationService>();

        #endregion

        private GameObject _currentModificationPrefab;

        private void OnEnable()
        {
            _presetModificator.OnModificationPresetSelected.AddListener(OnNetworkPresetForRoomSelected);
        }

        private void Start()
        {
            _presetModificator.SelectModificationPreset(_roomType, GetPresetNumber(_roomType.ToString()));
        }

        private void OnDisable()
        {
            _presetModificator.OnModificationPresetSelected.RemoveListener(OnNetworkPresetForRoomSelected);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_localPlayerHolder.IsOtherPlayerEqualsLocalPlayer(other.gameObject))
            {
                return;
            }

            ProvidePresetsAsset();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_localPlayerHolder.IsOtherPlayerEqualsLocalPlayer(other.gameObject))
            {
                return;
            }

            _presetModificator.RemovePresetsAsset();
        }

        private void ProvidePresetsAsset()
        {
            _presetModificator.ProvideModificationPresetsAsset(_presetsModificationAsset);
        }

        private void OnNetworkPresetForRoomSelected(RoomType roomType, int presetNumber)
        {
            if (roomType != _roomType)
            {
                return;
            }

            Destroy(_currentModificationPrefab);
            _currentModificationPrefab = Instantiate(_presetsModificationAsset.Presets.Find(preset => preset.PresetNumber == presetNumber).PresetPrefab);
        }

        private int GetPresetNumber(string presetName)
        {
            return _roomCustomProperties.CustomPropertiesContainKey(presetName)
                ? Convert.ToInt32(_roomCustomProperties.GetRoomCustomProperty(presetName))
                : DefaultPresetNumber;
        }
    }
}