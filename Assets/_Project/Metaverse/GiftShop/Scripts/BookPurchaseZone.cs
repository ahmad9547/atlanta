using Metaverse.GiftShop.PurchaseItem;

namespace Metaverse.GiftShop
{
    public sealed class BookPurchaseZone : PurchaseZone<BookInfoAsset>
    {
        protected override void ProvidePurchaseItemsAsset()
        {
            _purchaseItemHolder.ProvideBookAsset(_purchaseItemInfoAsset);
        }
    }
}
