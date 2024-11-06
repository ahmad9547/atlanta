using System;
using Avatars.Enums;
using Core.ServiceLocator;
using UnityEngine.Events;

namespace Avatars.Services
{
    public interface IAvatarAnimatorService : IService
    {
        UnityEvent<AnimatorOverrideControllerType> OnOverrideAnimatorController { get; }

        UnityEvent<AnimatorOverrideControllerType, int> OnOverrideAnimatorControllerPUN { get; }

        void OverrideAnimatorController(AnimatorOverrideControllerType controllerType);
        void OverrideAnimatorControllerPUN(AnimatorOverrideControllerType controllerType, int photonViewId);
    }
}