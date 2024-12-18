using Avatars.Enums;
using Core.ServiceLocator;
using UnityEngine.Events;

namespace Avatars.Services
{
    public interface IAvatarAnimatorService : IService
    {
        UnityEvent<AnimatorOverrideControllerType> OnOverrideAnimatorController { get; }

        void OverrideAnimatorController(AnimatorOverrideControllerType controllerType);
    }
}