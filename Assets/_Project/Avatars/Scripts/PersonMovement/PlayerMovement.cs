using Avatars.PersonMovement.MoveSettings;
using Avatars.PersonMovement.Services;
using Avatars.PersonMovement.States;
using Avatars.WebGLMovement;
using Core.ServiceLocator;
using LocationsManagement.Interfaces;
using Photon.Pun;
using UnityEngine;
using DG.Tweening;

namespace Avatars.PersonMovement
{
    public sealed class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private Transform _playerBody;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PhotonView _photonView;
        public GameObject handView;

        [Header("Grounded settings")]
        [SerializeField] private Vector3 _groundedOffset = new Vector3(0f, 0f, 0f);
        [SerializeField] private float _groundedRadius = 0.05f;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Color _groundedColor;

        public MoveSettingHolder moveSettings;

        public Rigidbody PlayerRigidbody => _playerRigidbody;

        public Transform PlayerBody => _playerBody;

        public PlayerMovementInput PlayerMovementInput => _playerMovementInput;

        public IPlayerAnimator PlayerAnimator { get; private set; }

        public bool IsGrounded { get; private set; }
        public bool IsPlayerSeating { get; set; }

        private PlayerMovementStateMachine _movementStateMachine;
        private PlayerMovementInput _playerMovementInput;

        private bool _isMovementEnabled = true;

        private MoveSettingHolder _moveSettingsHolder;

        #region Services

        private IPlayerMovementLockerService _playerMovementLockerInstance;

        private IPlayerMovementLockerService _playerMovementLocker
            => _playerMovementLockerInstance ??= Service.Instance.Get<IPlayerMovementLockerService>();

        private ILocationLoaderService _locationLoaderInstance;

        private ILocationLoaderService _locationLoader
            => _locationLoaderInstance ??= Service.Instance.Get<ILocationLoaderService>();

        #endregion

        private void Awake()
        {
            AddPlayerMovementInput();

            PlayerAnimator = _playerAnimator.GetComponent<IPlayerAnimator>();
        }

        private void OnEnable()
        {
            _playerMovementLocker.OnMovementLockedStateChanged.AddListener(LockPlayerMovement);
        }

        private void Start()
        {
            GetMovementSettings();
            InitializeStateMachine();
        }

        private void Update()
        {
            if (!_photonView.IsMine || !_isMovementEnabled)
            {
                return;
            }

            SelectMoveState();
            CallStateMachineUpdate();

   //         if (Input.GetKeyDown(KeyCode.C))
   //         {
   //             isCrouching = !isCrouching;

			//	GameObject.FindObjectOfType<CameraSwitcher>().crouchCamera.gameObject.SetActive(isCrouching);
   //             PlayerAnimator.ResetAnimator();
   //             PlayerAnimator.SetCrouchParameter(isCrouching);
			//}
		}

        internal bool isCrouching = false;

        private void FixedUpdate()
        {
            if (!_photonView.IsMine || !_isMovementEnabled)
            {
                return;
            }

            GroundCheck();
            CallStateMachineFixedUpdate();
        }

        private void OnDisable()
        {
            _playerMovementLocker.OnMovementLockedStateChanged.RemoveListener(LockPlayerMovement);
        }

        public void FreezePlayerPhysics(bool state)
        {
            _playerRigidbody.velocity = Vector3.zero;
            _playerRigidbody.constraints = state ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.FreezeAll;
            _isMovementEnabled = state;
        }


        private void AddPlayerMovementInput()
        {
            if (!_photonView.IsMine || gameObject.GetComponent<PlayerMovementInput>() != null)
            {
                return;
            }

            _playerMovementInput = gameObject.AddComponent<PlayerMovementInput>();
        }

        private void GetMovementSettings()
        {
            _moveSettingsHolder = moveSettings;
        }

        private void InitializeStateMachine()
        {
            _movementStateMachine = new PlayerMovementStateMachine(this, _moveSettingsHolder);
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                if (isR)
                {
                    PlayerRigidbody.useGravity = true;
                    isR = false;
                }
            }
        }

        bool isR = false;

        private void SelectMoveState()
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                PlayerRigidbody.useGravity = true;
                isR = false;

            }

            if (Input.GetKey(KeyCode.R))
            {
                isR = true;
                Vector3 moveDirection = _playerBody.right * PlayerMovementInput.HorizontalInput + _playerBody.forward * PlayerMovementInput.VerticalInput;
                Vector3 moveVelocity = moveDirection * 15f * Time.fixedDeltaTime;
                PlayerRigidbody.useGravity = false;
                PlayerRigidbody.velocity =
                    new Vector3(moveVelocity.x,
                    0,
                    moveVelocity.z);

            }
            else
            {

                if (_playerMovementInput.IsFlying)
                {
                    _movementStateMachine.SwitchState<FlyState>();
                }
                else if (IsGrounded && _playerMovementInput.IsJumping)
                {
                    //_movementStateMachine.SwitchState<JumpState>();
                }
                else if (IsGrounded && _playerMovementInput.IsRunningInput())
                {
                    _movementStateMachine.SwitchState<RunState>();
                }
                else if (IsGrounded && _playerMovementInput.IsWalkingInput())
                {
                    _movementStateMachine.SwitchState<WalkState>();
                }
                else
                {
                    _movementStateMachine.SwitchState<IdleState>();
                }
            }


           
        }

        private void CallStateMachineUpdate()
        {
            _movementStateMachine.CurrentState.Update();
        }

        private void GroundCheck()
        {
            IsGrounded = Physics.CheckSphere(_playerBody.localPosition + _groundedOffset, _groundedRadius, _groundMask);
        }

        private void CallStateMachineFixedUpdate()
        {
            _movementStateMachine.CurrentState.FixedUpdate();
        }

        private void LockPlayerMovement(bool state)
        {
            if (IsPlayerSeating)
            {
                return;
            }
            
            if (!_photonView.IsMine)
            {
                return;
            }

            _movementStateMachine.SwitchState<IdleState>();
            _playerRigidbody.velocity = Vector3.zero;
            _isMovementEnabled = !state;
        }

        #region Debug
        private void OnDrawGizmos()
        {
            Gizmos.color = _groundedColor;

            if (_playerBody != null)
            {
                Gizmos.DrawSphere(_playerBody.localPosition + _groundedOffset, _groundedRadius);
            }
        }
        #endregion
    }
}
