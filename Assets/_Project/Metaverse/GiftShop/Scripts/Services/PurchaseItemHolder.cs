using Metaverse.GiftShop.Interfaces;
using Metaverse.GiftShop.PurchaseItem;
using UnityEngine.Events;

namespace Metaverse.GiftShop.Services
{
    public sealed class PurchaseItemHolder : IPurchaseItemHolderService
    {
        public UnityEvent<TshirtInfoAsset> OnTshirtAssetProvided { get; }
            = new UnityEvent<TshirtInfoAsset>();

        public UnityEvent<BookInfoAsset> OnBookAssetProvided { get; }
            = new UnityEvent<BookInfoAsset>();

        public UnityEvent OnPurchaseItemAssetRemoved { get; } = new UnityEvent();

        public void ProvideTshirtAsset(TshirtInfoAsset infoAsset)
        {
            OnTshirtAssetProvided?.Invoke(infoAsset);
        }

        public void ProvideBookAsset(BookInfoAsset infoAsset)
        {
            OnBookAssetProvided?.Invoke(infoAsset);
        }

        public void RemovePurchaseItemAsset()
        {
            OnPurchaseItemAssetRemoved?.Invoke();
        }
    }
}