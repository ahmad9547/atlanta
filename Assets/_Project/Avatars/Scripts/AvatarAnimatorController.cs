using System;
using Avatars.Enums;
using UnityEngine;

namespace Avatars.Scripts
{
    [Serializable]
    public class AvatarAnimatorController
    {
        [SerializeField] private AnimatorOverrideControllerType _controllerType;
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;

        public AnimatorOverrideControllerType ControllerType => _controllerType;
        public AnimatorOverrideController AnimatorOverrideController => _animatorOverrideController;
    }
}