using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.GiftShopMenu.Couters
{
    public class ItemAmountSelector : MonoBehaviour
    {
        protected const int StepValue = 1;
        
        [SerializeField] private int _minValue;
        [SerializeField] private Button _plusButton;
        [SerializeField] private Button _minusButton;
        [SerializeField] protected TMP_Text _amountText;
        
        private int _currentAmount;

        public int CurrentAmount => _currentAmount;

        public void Reset()
        {
            _currentAmount = _minValue;
            UpdateVisual();
        }
        
        public void AddAmount(int amount = StepValue)
        {
            _currentAmount += amount;
            UpdateVisual();
        }

        public void SubtractAmount(int amount = StepValue)
        {
            if (_currentAmount == _minValue)
            {
                return;
            }
            
            _currentAmount -= amount;
            UpdateVisual();
        }

        private void Awake()
        {
            _currentAmount = _minValue;
        }

        private void OnEnable()
        {
            _plusButton.onClick.AddListener(OnPlusButtonClick);
            _minusButton.onClick.AddListener(OnMinusButtonClick);
        }
        
        private void OnDisable()
        {
            _plusButton.onClick.RemoveListener(OnPlusButtonClick);
            _minusButton.onClick.RemoveListener(OnMinusButtonClick);
        }

        protected virtual void OnPlusButtonClick()
        {
            AddAmount();
        }

        protected virtual void OnMinusButtonClick()
        {
            SubtractAmount();
        }

        private void UpdateVisual()
        {
            _amountText.text = _currentAmount.ToString();
        }
    }
}
