using System;
using LocationsManagement.Enums;
using UnityEngine;

namespace PlayerUIScene.SideMenu
{
    [Serializable]
    public class PanelContent
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private LocationType _locationType;

        public GameObject Content => _content;
        public LocationType LocationType => _locationType;
    }
}
