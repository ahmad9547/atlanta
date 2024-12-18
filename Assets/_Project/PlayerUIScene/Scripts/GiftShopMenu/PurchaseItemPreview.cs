using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.GiftShopMenu
{
    public sealed class PurchaseItemPreview : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        public void Initialize(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}
