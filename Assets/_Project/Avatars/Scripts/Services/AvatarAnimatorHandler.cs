using Avatars.Enums;
using UnityEngine.Events;

namespace Avatars.Services
{
    public class AvatarAnimatorHandler : IAvatarAnimatorService
    {
        public UnityEvent<AnimatorOverrideControllerType> OnOverrideAnimatorController { get; }
            = new UnityEvent<AnimatorOverrideControllerType>();

        public void OverrideAnimatorController(AnimatorOverrideControllerType controllerType)
        {
            OnOverrideAnimatorController?.Invoke(controllerType);
        }
    }
}