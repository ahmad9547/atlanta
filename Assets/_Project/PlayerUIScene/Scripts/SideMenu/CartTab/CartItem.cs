using Core.ServiceLocator;
using Metaverse.GiftShop.Enums;
using Metaverse.GiftShop.Interfaces;
using PlayerUIScene.GiftShopMenu.Couters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.SideMenu.CartTab
{
    public sealed class CartItem : MonoBehaviour
    {
        private const string CurrencySuffix = "$";

        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _size;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private Image _image;
        [SerializeField] private CartItemAmountSelector _cartItemAmountSelector;
        [SerializeField] private GameObject _sizeParent;

        #region Services

        private ICartItemHolderService _cartItemHolderInstance;
        private ICartItemHolderService _cartItemHolder
            => _cartItemHolderInstance ??= Service.Instance.Get<ICartItemHolderService>();

        #endregion

        public CartItemModel CartItemModel { get; set; }
        public CartItemAmountSelector CartItemAmountSelector => _cartItemAmountSelector;

        private void OnEnable()
        {
            _cartItemAmountSelector.OnAmountDecreased.AddListener(OnAmountDecreased);
            _cartItemAmountSelector.OnAmountIncreased.AddListener(OnAmountIncreased);
        }

        private void OnDisable()
        {
            _cartItemAmountSelector.OnAmountDecreased.RemoveListener(OnAmountDecreased);
            _cartItemAmountSelector.OnAmountIncreased.RemoveListener(OnAmountIncreased);
        }

        public void Initialize(CartItemModel cartItemModel, int amount)
        {
            CartItemModel = cartItemModel;
            _name.text = cartItemModel.Name;
            _sizeParent.SetActive(cartItemModel.Size != PurchaseItemSize.None);
            _size.text = cartItemModel.Size.ToString();
            _image.sprite = cartItemModel.Preview;
            _cartItemAmountSelector.AddAmount(amount);
            UpdatePrice();
        }

        public void AddAmount(int amount)
        {
            CartItemAmountSelector.AddAmount(amount);
            UpdatePrice();
        }

        private void OnAmountIncreased(int amount)
        {
            _cartItemHolder.AddItemToCart(CartItemModel, amount);
            UpdatePrice();
        }

        private void OnAmountDecreased(int amount)
        {
            _cartItemHolder.RemoveItemFromCart(CartItemModel, amount);
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            _price.text = CurrencySuffix + CartItemModel.Price * _cartItemAmountSelector.CurrentAmount;
        }
    }
}