using PlayerUIScene.Expandables;
using UnityEngine;

namespace PlayerUIScene.SideMenu.Teleport
{
    [System.Serializable]
    public class TeleportTabContent : PanelContent
    {
        [SerializeField] private ExpandableMenu _expandableMenu;

        public ExpandableMenu ExpandableMenu => _expandableMenu;
    }
}
