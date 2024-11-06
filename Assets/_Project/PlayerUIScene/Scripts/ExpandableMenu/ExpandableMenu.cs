using System.Collections.Generic;
using Core.ServiceLocator;
using Metaverse.Teleport.Interfaces;
using UnityEngine;

namespace PlayerUIScene.Expandables
{
    public sealed class ExpandableMenu : MonoBehaviour
    {
        [SerializeField] private ExpandableWindow _mainWindow;
        [SerializeField] private List<ExpandableWindow> _expandableWindows;

        #region Services

        private ITeleportControllerService _mapTeleportControllerInstance;
        private ITeleportControllerService _mapTeleportController
            => _mapTeleportControllerInstance ??= Service.Instance.Get<ITeleportControllerService>();

        #endregion

        private void OnEnable()
        {
            _mapTeleportController.TeleportedWithMapEvent.AddListener(OnTeleportedWithMap);
        }

        private void OnDisable()
        {
            _mapTeleportController.TeleportedWithMapEvent.RemoveListener(OnTeleportedWithMap);
        }

        public void ShowMainWindow()
        {
            CollapseAllWindows();
            _mainWindow.Expand();
        }

        private void OnTeleportedWithMap()
        {
            ShowMainWindow();
        }

        private void CollapseAllWindows()
        {
            foreach (ExpandableWindow window in _expandableWindows)
            {
                window.Collapse();
            }
        }
    }
}