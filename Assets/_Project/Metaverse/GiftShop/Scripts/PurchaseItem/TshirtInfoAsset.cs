using System.Collections.Generic;
using UnityEngine;

namespace Metaverse.GiftShop.PurchaseItem
{
    [CreateAssetMenu(fileName = "TshirtInformationAsset", menuName = "ScriptableObjects/PurchaseItemInfoAsset/TshirtInformationAsset")]
    public class TshirtInfoAsset : PurchaseItemInfoAsset
    {
        [SerializeField] private List<PurchaseItemInformation> _tshirtItemsInformation = new List<PurchaseItemInformation>();

        public List<PurchaseItemInformation> TshirtItemsInformation => _tshirtItemsInformation;
    }
}