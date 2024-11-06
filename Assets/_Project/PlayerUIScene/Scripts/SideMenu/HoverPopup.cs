using UnityEngine;

namespace PlayerUIScene.SideMenu
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class HoverPopup : MonoBehaviour
    {
        private const float HoverCheckInterval = 0.1f;

        [SerializeField] private GameObject _popup;

        private RectTransform _currentRectTransform;

        private float _hoverCheckTimer;

        private void Awake()
        {
            _currentRectTransform = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            UpdateHoverCheckTimer();
        }

        private void UpdateHoverCheckTimer()
        {
            _hoverCheckTimer += Time.deltaTime;

            if (_hoverCheckTimer < HoverCheckInterval)
            {
                return;
            }

            CheckHover();
            _hoverCheckTimer = 0f;
        }

        private void CheckHover()
        {
            Vector2 mousePosition = _currentRectTransform.InverseTransformPoint(Input.mousePosition);

            _popup.SetActive(_currentRectTransform.rect.Contains(mousePosition));
        }
    }
}
