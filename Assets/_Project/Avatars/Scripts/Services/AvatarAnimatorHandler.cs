using System;
using Avatars.Enums;
using UnityEngine.Events;

namespace Avatars.Services
{
    public class AvatarAnimatorHandler : IAvatarAnimatorService
    {
        public UnityEvent<AnimatorOverrideControllerType> OnOverrideAnimatorController { get; }
            = new UnityEvent<AnimatorOverrideControllerType>();

        public UnityEvent<AnimatorOverrideControllerType, int> OnOverrideAnimatorControllerPUN  { get; }
            = new UnityEvent<AnimatorOverrideControllerType, int>();

        public void OverrideAnimatorController(AnimatorOverrideControllerType controllerType)
        {
            OnOverrideAnimatorController?.Invoke(controllerType);
        }

        public void OverrideAnimatorControllerPUN(AnimatorOverrideControllerType controllerType, int photonViewId)
        {
            OnOverrideAnimatorControllerPUN?.Invoke(controllerType, photonViewId);
        }
    }
}