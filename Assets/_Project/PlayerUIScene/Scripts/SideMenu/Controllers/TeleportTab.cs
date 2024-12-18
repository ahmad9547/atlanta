using System.Collections.Generic;
using _Project.PlayerUIScene.Scripts.SideMenu.Services;
using Core.ServiceLocator;
using Core.UI;
using LocationsManagement.Enums;
using LocationsManagement.Interfaces;
using Metaverse.Teleport;
using Metaverse.Teleport.Interfaces;
using PlayerUIScene.SideMenu.Teleport;
using UnityEngine;

namespace PlayerUIScene.SideMenu.Controllers
{
    public sealed class TeleportTab : UIController
    {
        [SerializeField] private List<TeleportTabContent> _teleportTabContents;

        #region Services

        private ILocationLoaderService _locationLoaderInstance;
        private ILocationLoaderService _locationsLoader
            => _locationLoaderInstance ??= Service.Instance.Get<ILocationLoaderService>();

        private ISideMenuService _sideMenuInstance;
        private ISideMenuService _sideMenu
            => _sideMenuInstance ??= Service.Instance.Get<ISideMenuService>();

        private ITeleportControllerService _mapTeleportControllerInstance;
        private ITeleportControllerService _mapTeleportController
            => _mapTeleportControllerInstance ??= Service.Instance.Get<ITeleportControllerService>();

        #endregion

        protected override void Awake()
        {
            base.Awake();
            SetTabContent(_locationsLoader.LoadedLocation.LocationType);
        }

        public void SelectMapTeleportPoint(TeleportPointType teleportPointName)
        {
            _sideMenu.ChangeMenuPositionToClosed();
            _mapTeleportController.TeleportToMapPoint(teleportPointName);
        }

        private void SetTabContent(LocationType locationType)
        {
            foreach (var teleportTabContent in _teleportTabContents)
            {
                teleportTabContent.ExpandableMenu.ShowMainWindow();
                teleportTabContent.Content.SetActive(teleportTabContent.LocationType == locationType);
            }
        }
    }
}