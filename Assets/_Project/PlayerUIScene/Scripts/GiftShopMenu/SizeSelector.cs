using System.Collections.Generic;
using Metaverse.GiftShop.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.GiftShopMenu
{
    public sealed class SizeSelector : MonoBehaviour
    {
        private const PurchaseItemSize DefaultSize = PurchaseItemSize.S;

        [SerializeField] private List<SizeToggle> _sizeToggles;

        private readonly Dictionary<PurchaseItemSize, Toggle> _sizeTogglesDictionary = new Dictionary<PurchaseItemSize, Toggle>();
        public PurchaseItemSize CurrentItemSize { get; set; }

        private void Awake()
        {
            foreach (var sizeToggle in _sizeToggles)
            {
                _sizeTogglesDictionary.Add(sizeToggle.PurchaseItemSize, sizeToggle.Toggle);
            }
        }

        private void OnEnable()
        {
            foreach (SizeToggle toggle in _sizeToggles)
            {
                toggle.OnSwitchedOn.AddListener(SelectSize);
            }
        }

        private void OnDisable()
        {
            foreach (SizeToggle toggle in _sizeToggles)
            {
                toggle.OnSwitchedOn.RemoveListener(SelectSize);
            }
        }

        public void Reset()
        {
            _sizeTogglesDictionary[DefaultSize].isOn = true;
            SelectSize(DefaultSize);
        }

        private void SelectSize(PurchaseItemSize size)
        {
            CurrentItemSize = size;
        }
    }
}