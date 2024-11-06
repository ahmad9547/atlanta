using Core.ServiceLocator;
using Core.UI;
using LocationsManagement.Enums;
using LocationsManagement.Interfaces;

namespace PlayerUIScene.GiftShopMenu
{
    public sealed class GiftShopMenu : UIController
    {
        #region Services

        private ILocationLoaderService _locationLoaderInstance;
        private ILocationLoaderService _locationsLoader
            => _locationLoaderInstance ??= Service.Instance.Get<ILocationLoaderService>();

        #endregion
        
        protected override void Start()
        {
            base.Start();
            CheckLocationStatus();
        }

        private void CheckLocationStatus()
        {
            if (_locationsLoader.LoadedLocation.LocationType == LocationType.GiftShop)
            {
                Show();
            }
        }
    }
}
