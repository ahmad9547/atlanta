using Core.Helpers;
using Core.ServiceLocator;
using Core.UI;
using LocationsManagement.Enums;
using LocationsManagement.Interfaces;
using Metaverse.AreaModule;
using Metaverse.AreaModule.Services;
using TMPro;
using UnityEngine;

namespace PlayerUIScene.AreaModule
{
    public sealed class AreaMenu : UIController
    {
        private const string MeetingRoomName = "Meeting rooms";
        private const string Separator = " ";
        private const int DefaultValue = 0;

        [SerializeField] private TMP_Text _locationNameText;
        [SerializeField] private TMP_Text _areaNameText;
        [SerializeField] private TMP_Text _meetingRoomQuantityText;

        #region Services

        private ILocationLoaderService _locationLoaderInstance;
        private ILocationLoaderService _locationsLoader
            => _locationLoaderInstance ??= Service.Instance.Get<ILocationLoaderService>();

        private IAreaInformationHolderService _areaInformationHolderInstance;
        private IAreaInformationHolderService _areaInformationHolder
            => _areaInformationHolderInstance ??= Service.Instance.Get<IAreaInformationHolderService>();

        #endregion

        private void OnEnable()
        {
            _areaInformationHolder.OnPlayerAreaChanged.AddListener(UpdateVisual);
        }

        protected override void Start()
        {
            base.Start();
            SetLocationContent();
        }

        private void OnDisable()
        {
            _areaInformationHolder.OnPlayerAreaChanged.RemoveListener(UpdateVisual);
        }

        private void UpdateVisual(AreaInformation areaInformation)
        {
            _locationNameText.text = StringHelpers.GetSpaceSplitedString(areaInformation.LocationType);
            _areaNameText.text = StringHelpers.GetDashSplitedString(areaInformation.AreaName);

            if (areaInformation.MeetingRoomQuantity != DefaultValue)
            {
                _meetingRoomQuantityText.text = areaInformation.MeetingRoomQuantity + Separator + MeetingRoomName;
                _meetingRoomQuantityText.gameObject.SetActive(true);
                return;
            }

            _meetingRoomQuantityText.gameObject.SetActive(false);
        }

        private void SetLocationContent()
        {
            if (_locationsLoader.LoadedLocation.LocationType == LocationType.GWCCBuildingA ||
                _locationsLoader.LoadedLocation.LocationType == LocationType.GWCCBuildingB ||
                _locationsLoader.LoadedLocation.LocationType == LocationType.GWCCBuildingC)
            {
                UpdateVisual(_areaInformationHolder.CurrentAreaInformation);
                Show();
            }
        }
    }
}