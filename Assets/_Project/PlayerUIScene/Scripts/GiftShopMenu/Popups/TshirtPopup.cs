using Core.ServiceLocator;
using Metaverse.GiftShop.Enums;
using Metaverse.GiftShop.Interfaces;
using Metaverse.GiftShop.PurchaseItem;
using PlayerUIScene.GiftShopMenu.Couters;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.GiftShopMenu.Popups
{
    public sealed class TshirtPopup : PurchaseItemPopup
    {
        [SerializeField] private Button _cancelButton;
        [SerializeField] private SizeSelector _sizeSelector;
        [SerializeField] private ItemAmountSelector _itemAmountSelector;

        private TshirtInfoAsset _currentTshirtInfoAsset;
        private int _currentTshirtInformation;
        private PurchaseItemSize _currentPurchaseItemSize;

        #region Services

        private ICartItemHolderService _cartItemHolderInstance;
        private ICartItemHolderService _cartItemHolder
            => _cartItemHolderInstance ??= Service.Instance.Get<ICartItemHolderService>();

        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            _purchaseItemHolder.OnTshirtAssetProvided.AddListener(UpdatePopup);
            _purchaseItemHolder.OnPurchaseItemAssetRemoved.AddListener(ResetSelection);
            _cancelButton.onClick.AddListener(ResetSelection);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _purchaseItemHolder.OnTshirtAssetProvided.RemoveListener(UpdatePopup);
            _purchaseItemHolder.OnPurchaseItemAssetRemoved.RemoveListener(ResetSelection);
            _cancelButton.onClick.RemoveListener(ResetSelection);
        }

        protected override void UpdatePopupInformation(int index)
        {
            PurchaseItemInformation currentInformation = _currentTshirtInfoAsset.TshirtItemsInformation[index];

            _currentTshirtInformation = index;
            _name.text = currentInformation.Name;
            _description.text = currentInformation.Description;
            _price.text = CurrencySuffix + currentInformation.Price;
        }

        private void UpdatePopup(TshirtInfoAsset infoAsset)
        {
            base.UpdatePopup(infoAsset);

            if (_currentTshirtInfoAsset == infoAsset)
            {
                return;
            }

            _currentTshirtInfoAsset = infoAsset;
            CreateItemPreviews(infoAsset.TshirtItemsInformation);
            _scroller.UpdateScroller(infoAsset.TshirtItemsInformation.Count);
        }

        protected override void OnAddToCartButtonClick()
        {
            base.OnAddToCartButtonClick();
            _cartItemHolder.AddTshirtToCart(_currentTshirtInfoAsset.TshirtItemsInformation[_currentTshirtInformation],
                _sizeSelector.CurrentItemSize, _itemAmountSelector.CurrentAmount);
        }

        private void ResetSelection()
        {
            _itemAmountSelector.Reset();
            _sizeSelector.Reset();
        }
    }
}