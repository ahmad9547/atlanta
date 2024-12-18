using Metaverse.GiftShop.Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayerUIScene.GiftShopMenu
{
    public sealed class SizeToggle : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private PurchaseItemSize _purchaseItemSize;

        public Toggle Toggle => _toggle;
        public PurchaseItemSize PurchaseItemSize => _purchaseItemSize;

        [HideInInspector] public UnityEvent<PurchaseItemSize> OnSwitchedOn = new UnityEvent<PurchaseItemSize>();

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(OnValueChanged);
        }
        
        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(bool isOn)
        {
            if (isOn)
            {
                OnSwitchedOn?.Invoke(_purchaseItemSize);
            }
        }
    }
}
