using Core.ServiceLocator;
using Metaverse.GiftShop.Interfaces;
using Metaverse.GiftShop.PurchaseItem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.GiftShopMenu.Popups
{
    public sealed class BookPopup : PurchaseItemPopup
    {
        [SerializeField] private BookPreview _bookPreview;
        [SerializeField] private TMP_Text _dimensions;
        [SerializeField] private TMP_Text _pages;
        [SerializeField] private Button _previewButton;

        private BookInfoAsset _currentBookInfoAsset;
        private int _currentBookInformation;

        #region Services

        private ICartItemHolderService _cartItemHolderInstance;
        private ICartItemHolderService _cartItemHolder
            => _cartItemHolderInstance ??= Service.Instance.Get<ICartItemHolderService>();

        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            _purchaseItemHolder.OnBookAssetProvided.AddListener(UpdatePopup);
            _previewButton.onClick.AddListener(OnPreviewButtonClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _purchaseItemHolder.OnBookAssetProvided.RemoveListener(UpdatePopup);
            _previewButton.onClick.RemoveListener(OnPreviewButtonClick);
        }

        protected override void UpdatePopupInformation(int index)
        {
            BookItemInformation currentInformation = _currentBookInfoAsset.BookItemsInformation[index];

            _currentBookInformation = index;
            _name.text = currentInformation.Name;
            _description.text = currentInformation.Description;
            _price.text = CurrencySuffix + currentInformation.Price;
            _dimensions.text = currentInformation.Dimensions;
            _pages.text = currentInformation.Pages.ToString();
        }

        private void UpdatePopup(BookInfoAsset infoAsset)
        {
            base.UpdatePopup(infoAsset);
            _bookPreview.Hide();

            if (_currentBookInfoAsset == infoAsset)
            {
                return;
            }

            _currentBookInfoAsset = infoAsset;
            CreateItemPreviews(infoAsset.BookItemsInformation);
            _scroller.UpdateScroller(infoAsset.BookItemsInformation.Count);
        }

        protected override void OnAddToCartButtonClick()
        {
            base.OnAddToCartButtonClick();
            _cartItemHolder.AddBookToCart(_currentBookInfoAsset.BookItemsInformation[_currentBookInformation]);
        }

        private void OnPreviewButtonClick()
        {
            _bookPreview.ShowBookPreview(_currentBookInfoAsset.BookItemsInformation[_currentBookInformation]);
        }
    }
}