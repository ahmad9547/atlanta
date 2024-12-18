using UnityEngine;

namespace Avatars.PersonMovement
{
    public sealed class PlayerMovementInput : MonoBehaviour
    {
        private const string HorizontalInputKeyWord = "Horizontal";
        private const string VerticalInputKeyWord = "Vertical";
        private const string JumpInput = "Jump";
        private const KeyCode RunKeyCode = KeyCode.LeftShift;
        private const KeyCode FlyKeyCode = KeyCode.F;

        public float HorizontalInput => _horizontalInput;
        public float VerticalInput => _verticalInput;
        public bool IsJumping => _isJumping;
        public bool IsFlying => _isFlying;

        private float _horizontalInput;
        private float _verticalInput;
        private bool _isJumping;
        private bool _isRunning;
        private bool _isFlying;

        private void Update()
        {
            GetInput();
        }

        public bool IsWalkingInput()
        {
            return _horizontalInput != 0 || _verticalInput != 0;
        }

        public bool IsRunningInput()
        {
            return _isRunning && _verticalInput > 0 && _horizontalInput == 0;
        }

        private void GetInput()
        {
            _horizontalInput = Input.GetAxis(HorizontalInputKeyWord);
            _verticalInput = Input.GetAxis(VerticalInputKeyWord);

            _isJumping = Input.GetButtonDown(JumpInput);
            _isRunning = Input.GetKey(RunKeyCode);

            _isFlying = Input.GetKey(FlyKeyCode);
        }
    }
}
