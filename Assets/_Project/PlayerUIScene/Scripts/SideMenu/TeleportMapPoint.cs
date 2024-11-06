using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Metaverse.Teleport;
using PlayerUIScene.SideMenu.Controllers;

namespace PlayerUIScene.SideMenu
{
    public sealed class TeleportMapPoint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TeleportPointType _teleportPoint;

        [SerializeField] private TeleportTab _teleportTab;

        [SerializeField] private Button _pointButton;

        [SerializeField] private Image _pointImage;
        [SerializeField] private Image _popupImage;
        [SerializeField] private CanvasGroup _popupImageCanvasGroup;

        private const float PointHoverAnimationDuration = 0.3f;
        private const float PointScaleMultiplier = 1.3f;
        private const float PopupYAnchoredOffset = 25f;

        private Vector3 _pointImageDefaultScale;
        private float _popupImageDefaultYPosition;

        private void Awake()
        {
            _pointImageDefaultScale = _pointImage.rectTransform.localScale;
            _popupImageDefaultYPosition = _popupImage.rectTransform.anchoredPosition.y;
        }

        private void OnEnable()
        {
            _pointButton.onClick.AddListener(OnTeleportMapPointClick);
        }

        private void OnDisable()
        {
            _pointButton.onClick.RemoveListener(OnTeleportMapPointClick);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointImage.rectTransform.DOScale(_pointImageDefaultScale * PointScaleMultiplier, PointHoverAnimationDuration);
            _popupImage.rectTransform.DOAnchorPosY(_popupImageDefaultYPosition + PopupYAnchoredOffset, PointHoverAnimationDuration);
            _popupImageCanvasGroup.DOFade(1f, PointHoverAnimationDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointImage.rectTransform.DOScale(_pointImageDefaultScale, PointHoverAnimationDuration);
            _popupImage.rectTransform.DOAnchorPosY(_popupImageDefaultYPosition, PointHoverAnimationDuration);
            _popupImageCanvasGroup.DOFade(0f, PointHoverAnimationDuration);
        }

        private void OnTeleportMapPointClick()
        {
            _teleportTab.SelectMapTeleportPoint(_teleportPoint);
        }
    }
}
