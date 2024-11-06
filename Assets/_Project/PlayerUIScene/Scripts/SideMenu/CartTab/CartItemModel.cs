using Metaverse.GiftShop.Enums;
using Metaverse.GiftShop.PurchaseItem;
using UnityEngine;

namespace PlayerUIScene.SideMenu.CartTab
{
    public sealed class CartItemModel
    {
        public int Id { get; }
        public string Name { get; }
        public int Price { get; }
        public PurchaseItemSize Size { get; }
        public Sprite Preview { get; }
        
        public CartItemModel(PurchaseItemInformation itemInformation, PurchaseItemSize size = PurchaseItemSize.None)
        {
            Id = itemInformation.Id;
            Name = itemInformation.Name;
            Price = itemInformation.Price;
            Size = size;
            Preview = itemInformation.PreviewImage;
        }
    }
}
