using System.Collections.Generic;
using UnityEngine;
using Core.UI;
using UnityEngine.UI;
using System;
using _Project.PlayerUIScene.Scripts.SideMenu.Services;
using Common.PlayerInput.Interfaces;
using Core.ServiceLocator;
using PlayerUIScene.Services;

namespace PlayerUIScene.SideMenu.Controllers
{
    public sealed class SideMenuController : UIController
    {
        [SerializeField] private Button _screenFreeSpaceButton;

        [SerializeField] private Image _openMenuImage;
        [SerializeField] private Image _closeMenuImage;

        [SerializeField] private RectTransform _movableMenu;

        [SerializeField] private MenuPosition _closedMenuPosition;
        [SerializeField] private MenuPosition _openedMenuPosition;

        [SerializeField] private List<MenuOptionButton> _menuOptionButtons = new List<MenuOptionButton>();
        [SerializeField] private MenuOptionButton _defaultVisibleOptionButton;

        #region Services

        private IPlayerInputEventHandler _playerInputEventHandlerInstance;
        private IPlayerInputEventHandler _playerInputEventHandler
            => _playerInputEventHandlerInstance ??= Service.Instance.Get<IPlayerInputEventHandler>();

        private IMenuService _menuServiceInstance;
        private IMenuService _menuService
            => _menuServiceInstance ??= Service.Instance.Get<IMenuService>();

        private ISideMenuService _sideMenuInstance;
        private ISideMenuService _sideMenu
            => _sideMenuInstance ??= Service.Instance.Get<ISideMenuService>();
        private IWelcomeScreenStateHolderService _welcomeScreenStateHolderInstance;
        private IWelcomeScreenStateHolderService _welcomeScreenStateHolder
            => _welcomeScreenStateHolderInstance ??= Service.Instance.Get<IWelcomeScreenStateHolderService>();

        #endregion

        private MenuPosition _currentMenuPosition;

        private void OnEnable()
        {
            _playerInputEventHandler.OnSideMenuSwitch.AddListener(OnSideMenuInput);
            _screenFreeSpaceButton.onClick.AddListener(ChangeMenuPositionToClosed);
            _menuOptionButtons.ForEach(menuOption => menuOption.OnMenuOptionSelected += SelectMenuOption);
            _menuService.OnSideMenuClosed.AddListener(ChangeMenuPositionToClosed);

            _sideMenu.ChangeMenuPositionToClosedEvent.AddListener(ChangeMenuPositionToClosed);
            _sideMenu.ChangeMenuPositionEvent.AddListener(ChangeMenuPosition);
        }

        protected override void Start()
        {
            Show();
            _currentMenuPosition = _closedMenuPosition;

            ChangePosition(_currentMenuPosition, changeInstantly: true);
            SetMenuButtonsStates(openMenuButtonState: true, false);
            _sideMenu.LockCursor();
        }

        private void OnDisable()
        {
            _playerInputEventHandler.OnSideMenuSwitch.RemoveListener(OnSideMenuInput);
            _screenFreeSpaceButton.onClick.RemoveListener(ChangeMenuPositionToClosed);
            _menuOptionButtons.ForEach(menuOption => menuOption.OnMenuOptionSelected -= SelectMenuOption);
            _menuService.OnSideMenuClosed.RemoveListener(ChangeMenuPositionToClosed);

            _sideMenu.ChangeMenuPositionToClosedEvent.RemoveListener(ChangeMenuPositionToClosed);
            _sideMenu.ChangeMenuPositionEvent.RemoveListener(ChangeMenuPosition);
        }

        private void ChangeMenuPosition(MenuPosition targetMenuPosition)
        {
            ChangePosition(targetMenuPosition);
        }

        private void ChangePosition(MenuPosition targetMenuPosition, bool changeInstantly = false)
        {
            if (_currentMenuPosition == null)
            {
                Debug.LogError("Current MenuPosition is null");
                return;
            }

            _currentMenuPosition.ChangePosition(_movableMenu, targetMenuPosition, changeInstantly,
                () => { _currentMenuPosition = targetMenuPosition; });
        }

        private void ChangeMenuPositionToClosed()
        {
            ChangePosition(_closedMenuPosition);
            SetMenuButtonsStates(openMenuButtonState: true, false);
            _sideMenu.LockCursor();
        }

        private void ChangeMenuPositionToOpened()
        {
            SelectMenuOption(_defaultVisibleOptionButton);
            ChangePosition(_openedMenuPosition);
            SetMenuButtonsStates(openMenuButtonState: false, true);
            _sideMenu.UnlockCursor();
        }

        private void SelectMenuOption(MenuOptionButton selectedMenuOption)
        {
            _menuOptionButtons.ForEach(menuOption => menuOption.SetActive(false));
            selectedMenuOption.SetActive(true);
        }

        private void OnSideMenuInput()
        {
            if (_welcomeScreenStateHolder.IsWelcomeScreenVisible)
            {
                return;
            }

            (_currentMenuPosition.Equals(_closedMenuPosition)
                ? (Action)ChangeMenuPositionToOpened
                : ChangeMenuPositionToClosed).Invoke();
        }

        private void SetMenuButtonsStates(bool openMenuButtonState, bool closeMenuButtonState)
        {
            _openMenuImage.gameObject.SetActive(openMenuButtonState);
            _closeMenuImage.gameObject.SetActive(closeMenuButtonState);
        }
    }
}