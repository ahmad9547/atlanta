using _Project.PlayerUIScene.Scripts.SideMenu.Services;
using UnityEngine;
using Core.ServiceLocator;
using UnityEngine.UI;

namespace PlayerUIScene.SideMenu
{
    public sealed class MenuPositionButton : MonoBehaviour
    {
        [SerializeField] private Button _changePositionButton;
        [SerializeField] private MenuPosition _targetMenuPosition;

        #region Services

        private ISideMenuService _sideMenuInstance;
        private ISideMenuService _sideMenu
            => _sideMenuInstance ??= Service.Instance.Get<ISideMenuService>();

        #endregion

        private void OnEnable()
        {
            _changePositionButton.onClick.AddListener(ChangeMenuPosition);
        }

        private void OnDisable()
        {
            _changePositionButton.onClick.RemoveListener(ChangeMenuPosition);
        }

        private void ChangeMenuPosition()
        {
            _sideMenu.ChangeMenuPosition(_targetMenuPosition);
        }
    }
}