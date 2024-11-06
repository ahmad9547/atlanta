using Metaverse.GiftShop.PurchaseItem;

namespace Metaverse.GiftShop
{
    public sealed class TshirtPurchaseZone : PurchaseZone<TshirtInfoAsset>
    {
        protected override void ProvidePurchaseItemsAsset()
        {
            _purchaseItemHolder.ProvideTshirtAsset(_purchaseItemInfoAsset);
        }
    }
}