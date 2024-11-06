using Core.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Metaverse.Teleport.CityGates
{
    public sealed class CityGateUI : MonoBehaviour
    {
        private const float GateNameAnimationDuration = 1f;
        private const int MaxAmountOfPointsVisibleOnPopup = 9;

        [SerializeField] private CityGateTeleportPlace _cityGateTeleportPlace;

        [Header("City Gate UI windows settings")]
        [SerializeField] private Canvas _gateNameCanvas;
        [SerializeField] private Canvas _popupCanvas;

        [SerializeField] private Transform _gateNameContent;
        [SerializeField] private Transform _popupContent;

        [Header("Buttons list settings")]
        [SerializeField] private Transform _teleportPointButtonsParent;

        [SerializeField] private PopupTeleportPointButton _popupTeleportPointButtonPrefab;

        public bool PopupIsVisible { get; private set; }

        private readonly UIAnimator _uiAnimator = new UIAnimator();

        private readonly KeyCode[] _buttonsKeyCodes = {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
        };

        public void HideGateName()
        {
            _uiAnimator.HideWindow(_gateNameContent, GateNameAnimationDuration, () =>
            {
                _gateNameCanvas.enabled = false;
                _gateNameCanvas.gameObject.SetActive(false);
            });
        }

        public void ShowGateName()
        {
            _gateNameCanvas.enabled = true;
            _gateNameCanvas.gameObject.SetActive(true);
            _uiAnimator.ShowWindow(_gateNameContent, GateNameAnimationDuration);
        }

        public void HidePopup()
        {
            _uiAnimator.HideWindow(_popupContent, () =>
            {
                _popupCanvas.enabled = false;
                PopupIsVisible = false;
                _popupContent.gameObject.SetActive(false);
            });
        }

        public void ShowPopup()
        {
            _popupCanvas.enabled = true;
            _popupContent.gameObject.SetActive(true);
            _uiAnimator.ShowWindow(_popupContent);
            PopupIsVisible = true;
        }

        public void FillPopupWithTeleportPoints(List<TeleportPoint> teleportPoints)
        {
            CheckAmountOfTeleportPointsForPopup(teleportPoints.Count);

            foreach ((TeleportPoint teleportPoint, KeyCode buttonsKeyCode, int counter) in _buttonsKeyCodes
                         .Zip(teleportPoints, (buttonsKeyCode, teleportPoint) => (buttonsKeyCode, teleportPoint))
                         .Select((tuple, i) => (tuple.teleportPoint, tuple.buttonsKeyCode, i)))
            {
                Instantiate(_popupTeleportPointButtonPrefab, _teleportPointButtonsParent)
                    .InitializeButton(this, counter, teleportPoint, buttonsKeyCode);
            }
        }

        public void TeleportPointButtonPressed(TeleportPoint teleportPoint)
        {
            _cityGateTeleportPlace.SelectTargetTeleportPoint(teleportPoint);
            HidePopup();
        }

        private void CheckAmountOfTeleportPointsForPopup(int teleportPointsAmountInList)
        {
            if (teleportPointsAmountInList > MaxAmountOfPointsVisibleOnPopup)
            {
                Debug.LogError($"Number of Teleport Points for popup creation is " +
                    $"more than available {MaxAmountOfPointsVisibleOnPopup} points");
            }
        }
    }
}
