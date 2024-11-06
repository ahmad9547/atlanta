using System.Collections.Generic;
using Avatars.Enums;
using Avatars.PersonMovement;
using Avatars.Scripts;
using Avatars.Services;
using Core.ServiceLocator;
using UnityEngine;

namespace Avatars.WebGLMovement
{
	public sealed class PlayerAnimator : MonoBehaviour, IPlayerAnimator
	{
        [SerializeField] private Animator _animator;
        [SerializeField] private List<AvatarAnimatorController> _animatorControllers = new();

        private readonly int _moveXParameterId = Animator.StringToHash("MoveX");
        private readonly int _moveZParameterId = Animator.StringToHash("MoveZ");
        private readonly int _jumpTriggerId = Animator.StringToHash("Jump");
        private readonly int _runParameterId = Animator.StringToHash("Run");
        private readonly int _flyParameterId = Animator.StringToHash("Fly");
        private readonly int _crouchParameterId = Animator.StringToHash("Crouch");
        private readonly int _seatOnBenchParameterId = Animator.StringToHash("SeatOnBench");
        private readonly int _seatOnTableChairParameterId = Animator.StringToHash("SeatOnTableChair");
        private readonly int _seatOnAdminChairParameterId = Animator.StringToHash("SeatOnAdminChair");
        private readonly int _speakerParameterId = Animator.StringToHash("Speaker");
        private readonly int _resetTriggerId = Animator.StringToHash("Reset");

        #region Services

        private IAvatarAnimatorService _avatarAnimatorInstance;
        private IAvatarAnimatorService _avatarAnimatorHandler
            => _avatarAnimatorInstance ??= Service.Instance.Get<IAvatarAnimatorService>();

        #endregion

        private void OnEnable()
        {
            _avatarAnimatorHandler.OnOverrideAnimatorController.AddListener(OverrideController);
            _avatarAnimatorHandler.OnOverrideAnimatorControllerPUN.AddListener(OverrideControllerPUN);
        }

        private void OnDisable()
        {
            _avatarAnimatorHandler.OnOverrideAnimatorController.RemoveListener(OverrideController);
            _avatarAnimatorHandler.OnOverrideAnimatorControllerPUN.RemoveListener(OverrideControllerPUN);
        }

        public void SetupAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void SetMovementParameters(float moveX, float moveZ)
        {
            _animator.SetFloat(_moveXParameterId, moveX);
            _animator.SetFloat(_moveZParameterId, moveZ);
        }

        public void SetJumpParameter()
        {
            _animator.SetTrigger(_jumpTriggerId);
        }

        public void SetRunParameter(bool run)
        {
            _animator.SetBool(_runParameterId, run);
        }

        public void SetFlyParameter(bool fly)
        {
            _animator.SetBool(_flyParameterId, fly);
        }

        public void SetSeatingOnBenchParameter(bool state)
        {
            _animator.SetBool(_seatOnBenchParameterId, state);
        }

        public void SetSeatingOnTableChairParameter(bool state)
        {
            _animator.SetBool(_seatOnTableChairParameterId, state);
        }

        public void SetSeatingOnAdminChairParameter(bool state)
        {
            _animator.SetBool(_seatOnAdminChairParameterId, state);
        }

        public void SetSpeakerParameter(bool state)
        {
            _animator.SetBool(_speakerParameterId, state);
        }

        public void ResetAnimator()
        {
            SetCrouchParameter(false);
            SetRunParameter(false);
            SetSpeakerParameter(false);
            SetSeatingOnBenchParameter(false);
            SetSeatingOnTableChairParameter(false);
            SetSeatingOnAdminChairParameter(false);
            _animator.SetTrigger(_resetTriggerId);
            SetMovementParameters(0f, 0f);
        }

        public void OverrideController(AnimatorOverrideControllerType controllerType)
        {
            _animator.runtimeAnimatorController =
                _animatorControllers.Find(controller => controller.ControllerType == controllerType).AnimatorOverrideController;
        }
        
        private void OverrideControllerPUN(AnimatorOverrideControllerType controllerType, int photonViewId)
        {
            _animator.runtimeAnimatorController =
                _animatorControllers.Find(controller => controller.ControllerType == controllerType).AnimatorOverrideController;
        }

		public void SetCrouchParameter(bool crouch)
		{
			_animator.SetBool(_crouchParameterId, crouch);
		}
	}
}
