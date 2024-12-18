using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Metaverse.Teleport.Database;
using Core.UI;

namespace Metaverse.Teleport.CityGates
{
    public sealed class PopupTeleportPointButton : MonoBehaviour
    {
        [SerializeField] private Button _teleportPointButton;

        [SerializeField] private TextMeshProUGUI _pointName;
        [SerializeField] private TextMeshProUGUI _pointNumber;

        private UIAnimator _uiAnimator = new UIAnimator();

        private CityGateUI _cityGateUI;
        private TeleportPoint _buttonTeleportPoint;
        private KeyCode _buttonPressKeyCode = KeyCode.None;

        private void Update()
        {
            CheckButtonPress();
        }

        public void InitializeButton(CityGateUI cityGateUI, int buttonNumber, TeleportPoint teleportPoint, KeyCode buttonKeyCode)
        {
            _cityGateUI = cityGateUI;
            _pointNumber.SetText((buttonNumber + 1).ToString());
            _pointName.gameObject.AddComponent<TeleportPointNameApplier>().SetNameType(teleportPoint.TeleportPointType);
            _buttonTeleportPoint = teleportPoint;
            _buttonPressKeyCode = buttonKeyCode;
        }

        private void CheckButtonPress()
        {
            if (!_cityGateUI.PopupIsVisible || _buttonPressKeyCode == KeyCode.None)
            {
                return;
            }

            if (Input.GetKeyDown(_buttonPressKeyCode))
            {
                ButtonPressed();
            }
        }

        private void ButtonPressed()
        {
            _uiAnimator.ButtonScale(_teleportPointButton, () =>
            {
                _cityGateUI.TeleportPointButtonPressed(_buttonTeleportPoint);
            });
        }
    }
}
