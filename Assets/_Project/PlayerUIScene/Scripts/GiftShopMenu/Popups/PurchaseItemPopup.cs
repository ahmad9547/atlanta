using System.Collections.Generic;
using Core.ServiceLocator;
using Metaverse.GiftShop.Interfaces;
using Metaverse.GiftShop.PurchaseItem;
using PlayerUIScene.SideMenu.FurnitureArrangementPanel.Scrolling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.GiftShopMenu.Popups
{
    public abstract class PurchaseItemPopup : Popup
    {
        protected const string CurrencySuffix = "$";
        private const int DefaultValue = 0;

        [SerializeField] protected TMP_Text _name;
        [SerializeField] protected TMP_Text _description;
        [SerializeField] protected TMP_Text _price;
        [SerializeField] protected Scroller _scroller;
        [SerializeField] private Button _addToCartButton;
        [SerializeField] private GameObject _successPopup;
        [SerializeField] private PurchaseItemPreview _purchaseItemPreviewPrefab;
        [SerializeField] private RectTransform _imagesParent;

        protected List<PurchaseItemPreview> _itemPreviews = new List<PurchaseItemPreview>();
        private bool _isSuccessPopupVisible;

        #region Services

        private IPurchaseItemHolderService _purchaseItemHolderInstance;

        protected IPurchaseItemHolderService _purchaseItemHolder
            => _purchaseItemHolderInstance ??= Service.Instance.Get<IPurchaseItemHolderService>();

        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            _purchaseItemHolder.OnPurchaseItemAssetRemoved.AddListener(OnItemInfoAssetRemoved);
            _addToCartButton.onClick.AddListener(OnAddToCartButtonClick);
            _scroller.OnNewScrollElementSelected.AddListener(UpdatePopupInformation);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _purchaseItemHolder.OnPurchaseItemAssetRemoved.RemoveListener(OnItemInfoAssetRemoved);
            _addToCartButton.onClick.RemoveListener(OnAddToCartButtonClick);
            _scroller.OnNewScrollElementSelected.RemoveListener(UpdatePopupInformation);
        }

        protected abstract void UpdatePopupInformation(int index);

        protected void UpdatePopup(PurchaseItemInfoAsset infoAsset)
        {
            ShowPopup();
        }

        protected virtual void OnAddToCartButtonClick()
        {
            if (_isSuccessPopupVisible)
            {
                return;
            }

            _successPopup.SetActive(true);
            _isSuccessPopupVisible = true;

            _uiAnimator.ShowAndHideWindow(_successPopup.transform, () =>
            {
                _isSuccessPopupVisible = false;
                _successPopup.SetActive(false);
            });
        }

        protected void CreateItemPreviews(List<PurchaseItemInformation> itemsInformation)
        {
            ClearItemPreviews();

            foreach (PurchaseItemInformation itemInformation in itemsInformation)
            {
                PurchaseItemPreview purchaseItemPreview = Instantiate(_purchaseItemPreviewPrefab, _imagesParent);
                purchaseItemPreview.Initialize(itemInformation.PreviewImage);
                _itemPreviews.Add(purchaseItemPreview);
            }
        }

        protected void CreateItemPreviews(List<BookItemInformation> itemsInformation)
        {
            ClearItemPreviews();
            foreach (PurchaseItemInformation itemInformation in itemsInformation)
            {
                PurchaseItemPreview purchaseItemPreview = Instantiate(_purchaseItemPreviewPrefab, _imagesParent);
                purchaseItemPreview.Initialize(itemInformation.PreviewImage);
                _itemPreviews.Add(purchaseItemPreview);
            }
        }

        private void ClearItemPreviews()
        {
            if (_itemPreviews.Count == DefaultValue)
            {
                return;
            }

            _itemPreviews.ForEach(preview => Destroy(preview.gameObject));
            _itemPreviews.Clear();
        }

        private void OnItemInfoAssetRemoved()
        {
            ClosePopup();
        }
    }
}