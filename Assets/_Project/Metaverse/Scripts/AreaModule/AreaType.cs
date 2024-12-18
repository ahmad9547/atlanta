using System;
using System.Collections.Generic;
using System.Linq;
using LocationsManagement.Enums;
using UnityEngine;

namespace Metaverse.AreaModule
{
    public sealed class AreaType : MonoBehaviour, ISerializationCallbackReceiver
    {
        private const string PropertyName = "AreasList";

        public static List<string> AreasList;

        [SerializeField] private LocationType _location;
        [SerializeField] [ListToPopup(typeof(AreaType), PropertyName)] private string _areaName;
        [SerializeField] private int _meetingRoomQuantity;

        private List<string> PopupList;
        private AreaInformation _areaInformation = new AreaInformation();

        public AreaInformation AreaInformation => _areaInformation;

        public void OnBeforeSerialize()
        {
            AreasList = PopupList;
        }

        public void OnAfterDeserialize() {}

        private void Start()
        {
            _areaInformation.LocationType = _location.ToString();
            _areaInformation.AreaName = _areaName;
            _areaInformation.MeetingRoomQuantity = _meetingRoomQuantity;
        }

        private void OnValidate()
        {
            UpdateLocalizationPointSelection();
        }

        private void UpdateLocalizationPointSelection()
        {
            if (_location == LocationType.GWCCBuildingA)
            {
                PopupList = Enum.GetNames(typeof(GWCCBuildingAAreaType)).ToList();
            }

            if (_location == LocationType.GWCCBuildingB)
            {
                PopupList = Enum.GetNames(typeof(GWCCBuildingBAreaType)).ToList();
            }
            
            if (_location == LocationType.GWCCBuildingC)
            {
                PopupList = Enum.GetNames(typeof(GWCCBuildingCAreaType)).ToList();
            }
        }
    }
}