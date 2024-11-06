using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Core.UI
{
    public class UIAnimator
    {
        private const float ButtonScaleDuration = 0.15f;
        private const float ButtonScaleMultiplier = 1.05f;

        private const float MenuOpenedXPosition = -225f;
        private const float MenuClosedXPosition = 120f;
        private const float MenuMoveDuration = 0.5f;

        private const float WindowAnimationDuration = 0.2f;
        private const float WindowAnimationDelay = 3f;

        private readonly Dictionary<RectTransform, Vector3> _normalScales = new Dictionary<RectTransform, Vector3>();

        #region Button animations
        public void ButtonScale(Button button, TweenCallback OnComplete = null, float scaleDuration = ButtonScaleDuration, float scaleMultiplier = ButtonScaleMultiplier)
        {
            ButtonScale(button.transform as RectTransform, scaleDuration, scaleMultiplier, OnComplete);
        }

        private void ButtonScale(RectTransform rectTransform, float scaleDuration, float scaleMultiplier, TweenCallback OnComplete = null)
        {
            if (!_normalScales.ContainsKey(rectTransform))
            {
                _normalScales[rectTransform] = rectTransform.localScale;
            }

            Vector3 normalScale = _normalScales[rectTransform];
            Vector3 endScale = _normalScales[rectTransform] * ButtonScaleMultiplier;
            rectTransform.DOScale(endScale, ButtonScaleDuration).OnComplete(() =>
            {
                rectTransform.DOScale(normalScale, ButtonScaleDuration).OnComplete(OnComplete);
            });
        }
        #endregion

        #region Windows animations
        public void ShowWindow(Transform transform, TweenCallback OnComplete = null)
        {
            AnimateWindow(transform as RectTransform, Vector3.zero, Vector3.one, WindowAnimationDuration, OnComplete);
        }

        public void ShowWindow(Transform transform, float duration, TweenCallback OnComplete = null)
        {
            AnimateWindow(transform as RectTransform, Vector3.zero, Vector3.one, duration, OnComplete);
        }

        public void HideWindow(Transform transform, TweenCallback OnComplete = null)
        {
            AnimateWindow(transform as RectTransform, Vector3.one, Vector3.zero, WindowAnimationDuration, OnComplete);
        }

        public void HideWindow(Transform transform, float duration, TweenCallback OnComplete = null)
        {
            AnimateWindow(transform as RectTransform, Vector3.one, Vector3.zero, duration, OnComplete);
        }

        public void ShowAndHideWindow(Transform transform, TweenCallback OnComplete = null)
        {
            transform.localScale = Vector3.zero;

            Sequence sequence = DOTween.Sequence();
            sequence
                .Join(transform.DOScale(Vector3.one, WindowAnimationDuration))
                .AppendInterval(WindowAnimationDelay)
                .Append(transform.DOScale(Vector3.zero, WindowAnimationDuration))
                .OnComplete(OnComplete);
        }

        private void AnimateWindow(RectTransform rectTransform, Vector3 startScale, Vector3 endScale, float duration, TweenCallback OnComplete = null)
        {
            rectTransform.localScale = startScale;

            rectTransform.DOScale(endScale, duration).OnComplete(() =>
            {
                rectTransform.DOScale(Vector3.one, 0f).OnComplete(OnComplete);
            });
        }
        #endregion

        #region Side menu animations
        public void ShowSideMenu(Transform transform, TweenCallback OnComplete = null)
        {
            MoveSideMenu(transform as RectTransform, MenuClosedXPosition, MenuOpenedXPosition, MenuMoveDuration, OnComplete);
        }

        public void HideSideMenu(Transform transform, TweenCallback OnComplete = null)
        {
            MoveSideMenu(transform as RectTransform, MenuOpenedXPosition, MenuClosedXPosition, MenuMoveDuration, OnComplete);
        }

        private void MoveSideMenu(RectTransform rectTransform, float menuStartPosition, float menuEndPosition, float moveDuration, TweenCallback OnComplete = null)
        {
            rectTransform.DOAnchorPosX(menuStartPosition, 0f).OnComplete(() =>
            {
                rectTransform.DOAnchorPosX(menuEndPosition, moveDuration).OnComplete(OnComplete);
            });
        }
        #endregion
    }
}