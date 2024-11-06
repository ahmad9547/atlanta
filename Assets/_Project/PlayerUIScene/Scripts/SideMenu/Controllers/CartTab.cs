using System.Collections.Generic;
using Core.ServiceLocator;
using Metaverse.GiftShop.Interfaces;
using PlayerUIScene.SideMenu.CartTab;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.SideMenu.Controllers
{
    public sealed class CartTab : MonoBehaviour
    {
        private const int MinValue = 1;
        private const string PurchaseData = "";

        [SerializeField] private CartItem _cartItemPrefab;
        [SerializeField] private RectTransform _cartItemsParent;
        [SerializeField] private Button _goToCheckoutButton;

        private List<CartItem> _purchaseItemListings = new List<CartItem>();

        #region Services

        private ICartItemHolderService _cartItemHolderInstance;
        private ICartItemHolderService _cartItemHolder
            => _cartItemHolderInstance ??= Service.Instance.Get<ICartItemHolderService>();

        private ICheckoutFrameCreatorService _checkoutFrameCreatorInstance;
        private ICheckoutFrameCreatorService _checkoutFrameCreator
            => _checkoutFrameCreatorInstance ??= Service.Instance.Get<ICheckoutFrameCreatorService>();

        #endregion

        private void OnEnable()
        {
            _cartItemHolder.OnItemAddedToCart.AddListener(OnPurchaseItemAddedToCart);
            _cartItemHolder.OnItemRemovedFromCart.AddListener(OnPurchaseItemRemovedFromCart);
            _goToCheckoutButton.onClick.AddListener(OnGoToCheckoutButtonClick);
        }

        private void OnDisable()
        {
            _cartItemHolder.OnItemAddedToCart.RemoveListener(OnPurchaseItemAddedToCart);
            _cartItemHolder.OnItemRemovedFromCart.RemoveListener(OnPurchaseItemRemovedFromCart);
            _goToCheckoutButton.onClick.RemoveListener(OnGoToCheckoutButtonClick);
        }

        private void OnPurchaseItemAddedToCart(CartItemModel cartItemModel, int amount)
        {
            CartItem cartItem = FindTargetListing(cartItemModel);

            if (cartItem == null)
            {
                CreateListing(cartItemModel, amount);
                return;
            }

            cartItem.AddAmount(amount);
        }

        private void OnPurchaseItemRemovedFromCart(CartItemModel cartItemModel, int amount)
        {
            CartItem cartItem = FindTargetListing(cartItemModel);

            if (cartItem.CartItemAmountSelector.CurrentAmount == MinValue)
            {
                RemoveListing(cartItem);
                return;
            }

            cartItem.CartItemAmountSelector.SubtractAmount(amount);
        }

        private void OnGoToCheckoutButtonClick()
        {
            //TODO add actual checkout data
            _checkoutFrameCreator.CreateCheckoutIFrame(PurchaseData);
        }

        private CartItem FindTargetListing(CartItemModel cartItemModel)
        {
            return _purchaseItemListings.Find(listing => listing.CartItemModel.Id == cartItemModel.Id
            && listing.CartItemModel.Size == cartItemModel.Size);
        }

        private void CreateListing(CartItemModel cartItemModel, int amount)
        {
            CartItem cartItem = Instantiate(_cartItemPrefab, _cartItemsParent);
            _purchaseItemListings.Add(cartItem);
            cartItem.Initialize(cartItemModel, amount);
        }

        private void RemoveListing(CartItem cartItem)
        {
            Destroy(cartItem.gameObject);
            _purchaseItemListings.Remove(cartItem);
        }
    }
}
