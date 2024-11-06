using Core.ServiceLocator;
using Metaverse.AreaModification.Services;
using PlayerUIScene.SideMenu.AreaModification;

namespace Metaverse.Banners
{
    public sealed class ModificationBanner : Banner
    {
        #region Services

        private IAreaModificator _areaModificatorInstance;
        private IAreaModificator _areaModificator
            => _areaModificatorInstance ??= Service.Instance.Get<IAreaModificator>();

        #endregion
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _areaModificator.OnStateChanged.AddListener(OnBannersViewChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _areaModificator.OnStateChanged.RemoveListener(OnBannersViewChanged);
        }
        
        private void OnBannersViewChanged(ModificationType modificationType, bool isOn)
        {
            if (modificationType != ModificationType.Banners)
            {
                return;
            }

            _temporaryImageParent.SetActive(isOn);
        }
    }
}