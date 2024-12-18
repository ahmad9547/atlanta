using System;
using System.Collections.Generic;
using System.Linq;
using LocationsManagement.Enums;
using Metaverse.Teleport.Enums;
using UnityEngine;

namespace Metaverse.Teleport
{
    public sealed class TeleportPointType : MonoBehaviour, ISerializationCallbackReceiver
    {
        private const string PropertyName = "TeleportPointList";

        public static List<string> TeleportPointList;

        [SerializeField] private LocationType _location;
        [SerializeField] [ListToPopup(typeof(TeleportPointType), PropertyName)] private string _teleportPointName;

        private List<string> PopupList;

        public LocationType LocationType => _location;
        public string PointType => _teleportPointName;

        public void OnBeforeSerialize()
        {
            TeleportPointList = PopupList;
        }

        public void OnAfterDeserialize() {}

        private void OnValidate()
        {
            UpdateTeleportPointSelection();
        }

        private void UpdateTeleportPointSelection()
        {
            switch (_location)
            {
                case LocationType.OlympicPark:
                    PopupList = Enum.GetNames(typeof(OlympicParkTeleportPoint)).ToList();
                    break;
                case LocationType.AndrewYoungInternationalBlvd:
                    PopupList = Enum.GetNames(typeof(AndrewYoungBlvdTeleportPoint)).ToList();
                    break;
                case LocationType.GWCCBuildingA:
                    PopupList = Enum.GetNames(typeof(GWCCBuildingATeleportPoint)).ToList();
                    break;
                case LocationType.GWCCBuildingB:
                    PopupList = Enum.GetNames(typeof(GWCCBuildingBStartupPoint)).ToList();
                    break;
                case LocationType.GWCCBuildingC:
                    PopupList = Enum.GetNames(typeof(GWCCBuildingCTeleportPoint)).ToList();
                    break;
                case LocationType.GiftShop:
                    PopupList = Enum.GetNames(typeof(GiftShopTeleportPoint)).ToList();
                    break;
                case LocationType.AtlantaConventionCenter:
                    PopupList = Enum.GetNames(typeof(AtlantaConventionCenterTeleportPoint)).ToList();
                    break;
                case LocationType.Atrium:
                    PopupList = Enum.GetNames(typeof(AtriumTeleportPoint)).ToList();
                    break;
            }
        }
    }
}