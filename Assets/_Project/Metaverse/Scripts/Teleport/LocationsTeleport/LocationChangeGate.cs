using Core.Helpers;
using Core.UI;
using Metaverse.InteractionModule;
using Metaverse.InteractionModule.Interfaces;
using Metaverse.Teleport.CityGates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Metaverse.Teleport.LocationsTeleport
{
    public sealed class LocationChangeGate : LocationChangePlace, IInteractionZoneTrigger
    {
        [SerializeField] private CityGatePortalEffect _cityGatePortalEffect;
        [SerializeField] private InteractionZoneHandler _interactionZoneHandler;
        [SerializeField] private Canvas _popupCanvas;
        [SerializeField] private Image _changeLocationPopupImage;
        [SerializeField] private TextMeshProUGUI _locationTypeText;

        private UIAnimator _uiAnimator = new UIAnimator();

        protected override void OnEnable()
        {
            base.OnEnable();
            _interactionZoneHandler.AddInteractionZoneTriggerObserver(this);
        }

        private void Start()
        {
            _locationTypeText.text = StringHelpers.GetSpaceSplitedString(_destinationPointType.LocationType.ToString());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _interactionZoneHandler.RemoveInteractionZoneTriggerObserver(this);
        }

        public void OnInteractionZoneTriggerEnter(GameObject player)
        {
            if (_triggerZone.IsSomebodyElseInZone())
            {
                return;
            }

            _cityGatePortalEffect.Show();
            ShowPopup();
        }

        public void OnInteractionZoneTriggerExit(GameObject player)
        {
            if (!_triggerZone.IsNobodyInZone())
            {
                return;
            }

            _cityGatePortalEffect.Hide();
            HidePopup();
        }

        private void ShowPopup()
        {
            _popupCanvas.gameObject.SetActive(true);
            _uiAnimator.ShowWindow(_changeLocationPopupImage.transform);
        }

        private void HidePopup()
        {
            _uiAnimator.HideWindow(_changeLocationPopupImage.transform,
                () =>
                {
                    _popupCanvas.gameObject.SetActive(false);
                });
        }
    }
}