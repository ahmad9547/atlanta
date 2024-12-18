using Core.ServiceLocator;
using Core.UI;
using Metaverse.PresetModification;
using Metaverse.PresetModification.Interfaces;
using UnityEngine;
using UserManagement;

namespace PlayerUIScene.WorkshopMenu
{
    public sealed class WorkshopMenu : UIController
    {
        [SerializeField] private GameObject _workshopIcon;

        #region Services

        private IPresetModificationService _presetModificatorInstance;
        private IPresetModificationService _presetModificator
            => _presetModificatorInstance ??= Service.Instance.Get<IPresetModificationService>();

        private IUserProfileService _userProfileService;
        private IUserProfileService _userProfile
            => _userProfileService ??= Service.Instance.Get<IUserProfileService>();

        #endregion

        protected override void Start()
        {
            base.Start();
            CheckAdminStatus();
        }

        private void OnEnable()
        {
            _presetModificator.OnModificationPresetsAssetProvided.AddListener(ShowMenu);
            _presetModificator.OnPresetsAssetRemoved.AddListener(Hide);
        }

        private void OnDisable()
        {
            _presetModificator.OnModificationPresetsAssetProvided.RemoveListener(ShowMenu);
            _presetModificator.OnPresetsAssetRemoved.RemoveListener(Hide);
        }

        private void CheckAdminStatus()
        {
            _workshopIcon.SetActive(_userProfile.IsAdmin);
        }

        private void ShowMenu(PresetsModificationAsset furnitureAsset)
        {
            Show();
        }
    }
}