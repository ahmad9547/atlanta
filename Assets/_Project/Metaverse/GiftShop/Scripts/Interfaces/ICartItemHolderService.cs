using Core.ServiceLocator;
using Metaverse.GiftShop.Enums;
using Metaverse.GiftShop.PurchaseItem;
using PlayerUIScene.SideMenu.CartTab;
using UnityEngine.Events;

namespace Metaverse.GiftShop.Interfaces
{
    public interface ICartItemHolderService : IService
    {
        private const int DefaultPurchaseValue = 1;

        UnityEvent<CartItemModel, int> OnItemAddedToCart { get; }

        UnityEvent<CartItemModel, int> OnItemRemovedFromCart { get; }

        void AddItemToCart(CartItemModel item, int amount);

        void AddTshirtToCart(PurchaseItemInformation tshirtInformation, PurchaseItemSize size, int amount);

        void AddBookToCart(BookItemInformation bookInformation, int amount = DefaultPurchaseValue);

        void RemoveItemFromCart(CartItemModel cartItemModel, int amount = DefaultPurchaseValue);
    }
}