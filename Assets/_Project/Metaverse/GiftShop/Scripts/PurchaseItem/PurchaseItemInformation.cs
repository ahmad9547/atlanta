using System;
using UnityEngine;

namespace Metaverse.GiftShop.PurchaseItem
{
    [Serializable]
    public class PurchaseItemInformation
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private int _price;
        [SerializeField] private Sprite _previewImage;

        public int Id => _id;
        public string Name => _name;
        public string Description => _description;
        public int Price => _price;
        public Sprite PreviewImage => _previewImage;
    }
}