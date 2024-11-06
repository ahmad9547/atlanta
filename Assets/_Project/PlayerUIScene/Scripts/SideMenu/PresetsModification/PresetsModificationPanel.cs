using System.Collections.Generic;
using Core.ServiceLocator;
using Core.UI;
using Metaverse.PresetModification;
using Metaverse.PresetModification.Interfaces;
using PhotonEngine.CustomProperties.Services;
using PlayerUIScene.SideMenu.FurnitureArrangementPanel.Scrolling;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.SideMenu.PresetsModification
{
    public sealed class PresetsModificationPanel : UIController
    {
        private const int DefaultValue = 0;

        [SerializeField] private ModificationElement _modificationElement;
        [SerializeField] private Button _applyButton;
        [SerializeField] protected Scroller _scroller;
        [SerializeField] protected RectTransform _presetPreviewsParent;

        private int _currentPresetNumber;
        private List<GameObject> _previews = new List<GameObject>();
        private PresetsModificationAsset _modificationPresetAsset;
        private RoomType _currenRoomType;

        #region Services

        private IPresetModificationService _presetModificatorInstance;
        protected IPresetModificationService _presetModificator
            => _presetModificatorInstance ??= Service.Instance.Get<IPresetModificationService>();

        private IRoomCustomPropertiesService _roomCustomPropertiesService;
        protected IRoomCustomPropertiesService _roomCustomProperties
            => _roomCustomPropertiesService ??= Service.Instance.Get<IRoomCustomPropertiesService>();

        #endregion

        private void OnEnable()
        {
            _presetModificator.OnModificationPresetsAssetProvided.AddListener(OnModificationPresetAssetProvided);
            _presetModificator.OnPresetsAssetRemoved.AddListener(Hide);
            _scroller.OnNewScrollElementSelected.AddListener(OnNewElementSelected);
            _applyButton.onClick.AddListener(OnApplyButtonClicked);
        }

        private void OnDisable()
        {
            _presetModificator.OnModificationPresetsAssetProvided.RemoveListener(OnModificationPresetAssetProvided);
            _presetModificator.OnPresetsAssetRemoved.RemoveListener(Hide);
            _scroller.OnNewScrollElementSelected.RemoveListener(OnNewElementSelected);
            _applyButton.onClick.RemoveListener(OnApplyButtonClicked);
        }

        private void OnModificationPresetAssetProvided(PresetsModificationAsset furnitureAsset)
        {
            Show();
            _modificationPresetAsset = furnitureAsset;
            UpdateFurnitureSelection();
        }

        private void UpdateFurnitureSelection()
        {
            if (_currenRoomType == _modificationPresetAsset.RoomType)
            {
                return;
            }

            _currenRoomType = _modificationPresetAsset.RoomType;
            ClearPresetPreviews();

            foreach (ModificationPreset furniturePreset in _modificationPresetAsset.Presets)
            {
                ModificationElement modificationElement = Instantiate(_modificationElement, _presetPreviewsParent);
                modificationElement.Initialize(furniturePreset);
                _previews.Add(modificationElement.gameObject);
            }

            _scroller.UpdateScroller(_modificationPresetAsset.Presets.Count);
        }

        private void ClearPresetPreviews()
        {
            if (_previews.Count == DefaultValue)
            {
                return;
            }

            _previews.ForEach(furnitureArrangement => Destroy(furnitureArrangement));
            _previews.Clear();
        }

        private void OnNewElementSelected(int number)
        {
            _currentPresetNumber = number;
        }

        private void OnApplyButtonClicked()
        {
            _roomCustomProperties.AddOrUpdateRoomCustomProperty(_currenRoomType.ToString(), _currentPresetNumber);
            _presetModificator.SelectModificationPreset(_currenRoomType, _currentPresetNumber);
        }
    }
}