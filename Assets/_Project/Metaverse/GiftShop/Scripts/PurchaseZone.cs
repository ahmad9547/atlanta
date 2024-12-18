using Core.ServiceLocator;
using Core.UI;
using Metaverse.GiftShop.Interfaces;
using Metaverse.GiftShop.PurchaseItem;
using Metaverse.InteractionModule;
using Metaverse.InteractionModule.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Metaverse.GiftShop
{
    public abstract class PurchaseZone<TPurchaseItemInfoAsset> : MonoBehaviour, IInteractionZoneTrigger, IInteractionZoneButton
        where TPurchaseItemInfoAsset : PurchaseItemInfoAsset
    {
        [SerializeField] protected TPurchaseItemInfoAsset _purchaseItemInfoAsset;
        [SerializeField] private InteractionZoneHandler _interactionZoneHandler;
        [SerializeField] private Image _itemInfoImage;
        [SerializeField] private TMP_Text _itemInfoText;

        #region Services

        private IPurchaseItemHolderService _purchaseItemHolderInstance;
        protected IPurchaseItemHolderService _purchaseItemHolder
            => _purchaseItemHolderInstance ??= Service.Instance.Get<IPurchaseItemHolderService>();

        #endregion

        private UIAnimator _uiAnimator = new UIAnimator();
        
        private void Start()
        {
            _itemInfoText.text = _purchaseItemInfoAsset.Name;
        }

        private void OnEnable()
        {
            _interactionZoneHandler.AddInteractionZoneButtonObserver(this);
            _interactionZoneHandler.AddInteractionZoneTriggerObserver(this);
        }

        private void OnDisable()
        {
            _interactionZoneHandler.RemoveInteractionZoneButtonObserver(this);
            _interactionZoneHandler.RemoveInteractionZoneTriggerObserver(this);
        }

        public void OnInteractionZoneTriggerEnter(GameObject player)
        {
            _itemInfoImage.gameObject.SetActive(true);
            _uiAnimator.ShowWindow(_itemInfoImage.transform);
        }

        public void OnInteractionZoneTriggerExit(GameObject player)
        {
            _purchaseItemHolder.RemovePurchaseItemAsset();

            _uiAnimator.HideWindow(_itemInfoImage.transform, () =>
            {
                _itemInfoImage.gameObject.SetActive(false);
            });
        }

        public void StartInteractionButtonClick()
        {
            ProvidePurchaseItemsAsset();
        }

        public void EndInteractionButtonClick() { }

        protected abstract void ProvidePurchaseItemsAsset();
    }
}