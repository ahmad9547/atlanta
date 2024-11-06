using UnityEngine;

namespace Core.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIController : MonoBehaviour
    {
        public bool IsVisible { get { return _isVisible; } }

        protected readonly UIAnimator _uiAnimator = new UIAnimator();

        private bool _isVisible;

        private CanvasGroup _canvasGroup;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void Start()
        {
            Hide();
        }

        public virtual void Hide()
        {
            _isVisible = false;
            ChangeVisibility(_isVisible);
        }

        public virtual void Show()
        {
            _isVisible = true;
            ChangeVisibility(_isVisible);
        }

        private void ChangeVisibility(bool visible)
        {
            _canvasGroup.alpha = visible ? 1f : 0f;
            _canvasGroup.interactable = visible;
            _canvasGroup.blocksRaycasts = visible;
        }
    }
}