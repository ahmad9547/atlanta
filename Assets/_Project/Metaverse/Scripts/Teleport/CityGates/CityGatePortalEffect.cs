using UnityEngine;

namespace Metaverse.Teleport.CityGates
{
    public sealed class CityGatePortalEffect : MonoBehaviour
    {
        private const string PortalAppear = "PortalAppear";
        private const string PortalDisappear = "PortalDisappear";

        [SerializeField] private Animator _portalEffectAnimator;

        private int _portalAppearId;
        private int _portalDisappearId;

        private void Awake()
        {
            _portalAppearId = GetAnimationId(PortalAppear);
            _portalDisappearId = GetAnimationId(PortalDisappear);
        }

        public void Show()
        {
            _portalEffectAnimator.ResetTrigger(_portalDisappearId);
            _portalEffectAnimator.SetTrigger(_portalAppearId);

        }

        public void Hide()
        {
            _portalEffectAnimator.ResetTrigger(_portalAppearId);
            _portalEffectAnimator.SetTrigger(_portalDisappearId);
        }

        private int GetAnimationId(string animationName)
        {
            return Animator.StringToHash(animationName);
        }
    }
}
