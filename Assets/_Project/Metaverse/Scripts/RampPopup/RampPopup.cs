using Core.UI;
using Metaverse.InteractionModule;
using Metaverse.InteractionModule.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Metaverse.RampPopup
{
    public sealed class RampPopup : MonoBehaviour, IInteractionZoneTrigger
    {
        [SerializeField] private InteractionZoneHandler _interactionZoneHandler;

        [SerializeField] private Canvas _popupCanvas;
        [SerializeField] private Image _popupImage;

        private UIAnimator _uiAnimator = new UIAnimator();

        public void OnInteractionZoneTriggerEnter(GameObject player)
        {
            ShowPopup();
        }

        public void OnInteractionZoneTriggerExit(GameObject player)
        {
            HidePopup();
        }

        private void OnEnable()
        {
            _interactionZoneHandler.AddInteractionZoneTriggerObserver(this);
        }

        private void OnDisable()
        {
            _interactionZoneHandler.RemoveInteractionZoneTriggerObserver(this);
        }

        private void ShowPopup()
        {
            _popupCanvas.gameObject.SetActive(true);
            _uiAnimator.ShowWindow(_popupImage.transform);
        }

        private void HidePopup()
        {
            _uiAnimator.HideWindow(_popupImage.transform, () => { _popupCanvas.gameObject.SetActive(false); });
        }
    }
}