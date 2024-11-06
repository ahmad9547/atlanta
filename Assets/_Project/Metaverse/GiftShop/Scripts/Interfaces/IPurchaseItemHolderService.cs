using Core.ServiceLocator;
using Metaverse.GiftShop.PurchaseItem;
using UnityEngine.Events;

namespace Metaverse.GiftShop.Interfaces
{
    public interface IPurchaseItemHolderService : IService
    {
        UnityEvent<TshirtInfoAsset> OnTshirtAssetProvided { get; }
        UnityEvent<BookInfoAsset> OnBookAssetProvided { get; }

        UnityEvent OnPurchaseItemAssetRemoved { get; }

        void ProvideTshirtAsset(TshirtInfoAsset infoAsset);
        void ProvideBookAsset(BookInfoAsset infoAsset);

        void RemovePurchaseItemAsset();
    }
}