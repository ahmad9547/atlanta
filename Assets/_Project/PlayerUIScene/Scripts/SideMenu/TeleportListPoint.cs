using Metaverse.Teleport;
using PlayerUIScene.SideMenu.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.SideMenu
{
    public sealed class TeleportListPoint : MonoBehaviour
    {
        [SerializeField] private TeleportPointType _teleportPoint;

        [SerializeField] private TeleportTab _teleportTab;

        [SerializeField] private Button _pointButton;

        private void OnEnable()
        {
            _pointButton.onClick.AddListener(OnTeleportMapPointClick);
        }

        private void OnDisable()
        {
            _pointButton.onClick.RemoveListener(OnTeleportMapPointClick);
        }

        private void OnTeleportMapPointClick()
        {
            _teleportTab.SelectMapTeleportPoint(_teleportPoint);
        }
    }
}
