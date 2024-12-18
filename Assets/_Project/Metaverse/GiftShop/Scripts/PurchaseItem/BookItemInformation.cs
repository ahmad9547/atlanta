using System;
using UnityEngine;

namespace Metaverse.GiftShop.PurchaseItem
{
    [Serializable]
    public class BookItemInformation : PurchaseItemInformation
    {
        [SerializeField] private string _dimensions;
        [SerializeField] private int _pages;
        [SerializeField] private string _bookPdfName;
        [SerializeField] private string _bookPassword;

        public string Dimensions => _dimensions;
        public int Pages => _pages;
        public string BookPdfName => _bookPdfName;
        public string BookPassword => _bookPassword;
    }
}