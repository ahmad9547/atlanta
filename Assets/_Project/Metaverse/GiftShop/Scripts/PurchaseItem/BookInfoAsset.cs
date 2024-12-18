using System.Collections.Generic;
using UnityEngine;

namespace Metaverse.GiftShop.PurchaseItem
{
    [CreateAssetMenu(fileName = "BookInformationAsset", menuName = "ScriptableObjects/PurchaseItemInfoAsset/BookInformationAsset")]
    public sealed class BookInfoAsset : PurchaseItemInfoAsset
    {
        [SerializeField] private List<BookItemInformation> _bookItemsInformation = new List<BookItemInformation>();

        public List<BookItemInformation> BookItemsInformation => _bookItemsInformation;
    }
}