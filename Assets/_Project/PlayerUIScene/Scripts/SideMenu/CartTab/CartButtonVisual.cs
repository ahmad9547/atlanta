using Core.ServiceLocator;
using Metaverse.GiftShop.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.SideMenu.CartTab
{
    public sealed class CartButtonVisual : MonoBehaviour
    {
        private const int DefaultValue = 0;
    
        [SerializeField] private Button _makeOrderButton;
        [SerializeField] private GameObject _counterImage;
        [SerializeField] private TMP_Text _counterText;
        
        #region Services
        
        private ICartItemHolderService _cartItemHolderInstance;

        private ICartItemHolderService _cartItemHolder
            => _cartItemHolderInstance ??= Service.Instance.Get<ICartItemHolderService>();
        
        #endregion

        private int _currentAmount;
        
        private void OnEnable()
        {
            _cartItemHolder.OnItemAddedToCart.AddListener(OnPurchaseItemAddedToCart);
            _cartItemHolder.OnItemRemovedFromCart.AddListener(OnPurchaseItemRemovedFromCart);
        }
        
        private void OnDisable()
        {
            _cartItemHolder.OnItemAddedToCart.RemoveListener(OnPurchaseItemAddedToCart);
            _cartItemHolder.OnItemRemovedFromCart.RemoveListener(OnPurchaseItemRemovedFromCart);
        }

        private void OnPurchaseItemAddedToCart(CartItemModel cartItemModel, int amount)
        {
            _currentAmount += amount;
            _counterText.text = _currentAmount.ToString();
            _counterImage.SetActive(true);
            _makeOrderButton.gameObject.SetActive(true);
        }
        
        private void OnPurchaseItemRemovedFromCart(CartItemModel cartItemModel, int amount)
        {
            _currentAmount -= amount;
            _counterText.text = _currentAmount.ToString();

            if (_currentAmount <= DefaultValue)
            {
                _counterImage.SetActive(false);
                _makeOrderButton.gameObject.SetActive(false);
            }
        }
    }
}
