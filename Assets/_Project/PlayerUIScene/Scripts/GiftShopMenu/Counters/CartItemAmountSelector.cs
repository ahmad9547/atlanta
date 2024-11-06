using UnityEngine;
using UnityEngine.Events;

namespace PlayerUIScene.GiftShopMenu.Couters
{
    public sealed class CartItemAmountSelector : ItemAmountSelector
    {
        [HideInInspector] public UnityEvent<int> OnAmountDecreased = new UnityEvent<int>();
        [HideInInspector] public UnityEvent<int> OnAmountIncreased = new UnityEvent<int>();

        protected override void OnPlusButtonClick()
        {
            OnAmountIncreased?.Invoke(StepValue);
        }
        
        protected override void OnMinusButtonClick()
        {
            OnAmountDecreased?.Invoke(StepValue);
        }
    }
}