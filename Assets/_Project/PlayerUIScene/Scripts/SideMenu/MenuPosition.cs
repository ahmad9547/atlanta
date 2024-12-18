using UnityEngine;
using DG.Tweening;

namespace PlayerUIScene.SideMenu
{
    public sealed class MenuPosition : MonoBehaviour
    {
        private const float InstantDuration = 0f;

        [SerializeField] private float _menuAnchoredXPosition;

        public float MenuAnchoredPosition { get { return _menuAnchoredXPosition; } }

        private const float PositionChangePerSecond = 500f;
        private const float MinPositionChangeTime = 0.5f;
        private const float MaxPositionChangeTime = 1f;

        public void ChangePosition(RectTransform rectTransform, MenuPosition targetMenuPosition, bool changeInstantly = false, TweenCallback callback = null)
        {
            float duration = changeInstantly ? InstantDuration : GetMovePositionDuration(targetMenuPosition.MenuAnchoredPosition);

            rectTransform.DOAnchorPosX(targetMenuPosition.MenuAnchoredPosition, duration).OnComplete(() => callback?.Invoke());
        }

        public override bool Equals(object other)
        {
            if ((other == null) || !this.GetType().Equals(other.GetType()))
            {
                return false;
            }

            MenuPosition otherMenuPosition = (MenuPosition)other;
            return otherMenuPosition.MenuAnchoredPosition == this._menuAnchoredXPosition;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private float GetMovePositionDuration(float targetPosition)
        {
            float distance = Mathf.Abs(targetPosition - _menuAnchoredXPosition);
            float duration = distance / PositionChangePerSecond;

            return Mathf.Clamp(duration, MinPositionChangeTime, MaxPositionChangeTime);
        }
    }
}
