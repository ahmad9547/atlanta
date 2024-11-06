using Metaverse.GiftShop.Enums;
using Metaverse.GiftShop.Interfaces;
using Metaverse.GiftShop.PurchaseItem;
using PlayerUIScene.SideMenu.CartTab;
using UnityEngine.Events;

namespace Metaverse.GiftShop.Services
{
    public sealed class CartItemHolder : ICartItemHolderService
    {
        public UnityEvent<CartItemModel, int> OnItemAddedToCart { get; }
            = new UnityEvent<CartItemModel, int>();

        public UnityEvent<CartItemModel, int> OnItemRemovedFromCart { get; }
            = new UnityEvent<CartItemModel, int>();

        public void AddItemToCart(CartItemModel item, int amount)
        {
            OnItemAddedToCart?.Invoke(item, amount);
        }

        public void AddTshirtToCart(PurchaseItemInformation tshirtInformation, PurchaseItemSize size, int amount)
        {
            CartItemModel cartItemModel = new CartItemModel(tshirtInformation, size);
            OnItemAddedToCart?.Invoke(cartItemModel, amount);
        }

        public void AddBookToCart(BookItemInformation bookInformation, int amount)
        {
            CartItemModel cartItemModel = new CartItemModel(bookInformation);
            OnItemAddedToCart?.Invoke(cartItemModel, amount);
        }

        public void RemoveItemFromCart(CartItemModel cartItemModel, int amount)
        {
            OnItemRemovedFromCart?.Invoke(cartItemModel, amount);
        }
    }
}
