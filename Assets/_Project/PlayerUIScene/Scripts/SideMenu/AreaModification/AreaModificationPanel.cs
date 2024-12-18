using System.Collections.Generic;
using Core.ServiceLocator;
using LocationsManagement.Enums;
using LocationsManagement.Interfaces;
using UnityEngine;

namespace PlayerUIScene.SideMenu.AreaModification
{
    public sealed class AreaModificationPanel : MonoBehaviour
    {
        [SerializeField] private List<PanelContent> _contents;

        #region Services

        private ILocationLoaderService _locationLoaderInstance;
        private ILocationLoaderService _locationsLoader
            => _locationLoaderInstance ??= Service.Instance.Get<ILocationLoaderService>();

        #endregion

        private void Start()
        {
            SetPanelContent(_locationsLoader.LoadedLocation.LocationType);
        }

        private void SetPanelContent(LocationType locationType)
        {
            foreach (PanelContent content in _contents)
            {
                content.Content.SetActive(content.LocationType == locationType);
            }
        }
    }
}