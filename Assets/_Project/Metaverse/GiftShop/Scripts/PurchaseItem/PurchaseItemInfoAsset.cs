using UnityEngine;

namespace Metaverse.GiftShop.PurchaseItem
{
    public class PurchaseItemInfoAsset : ScriptableObject
    {
        [SerializeField] private string _name;

        public string Name => _name;
    }
}