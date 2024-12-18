using Avatars.WebGLMovement.MouseControll.Interfaces;
using UnityEngine;
using Core.ServiceLocator;

namespace Avatars.WebGLMovement.MouseControll
{
    public abstract class MouseLook : MonoBehaviour
    {
        [SerializeField] protected Transform _cameraRoot;

        [Header("Sensitivity")]
        [SerializeField] protected float _mouseYSensitivity = 1f;
        [SerializeField] protected float _mouseXSensitivity = 1f;

        #region Constants
        private const string MouseXInputAxis = "Mouse X";
        private const string MouseYInputAxis = "Mouse Y";

        private const float MouseInputMultiplier = 100f;
        #endregion

        #region Services
        
        private IMouseCursorService _mouseCursorInstance;
        private IMouseCursorService _mouseCursor
            => _mouseCursorInstance ??= Service.Instance.Get<IMouseCursorService>();
        
        #endregion
        
        protected float _cameraXRotation = 0f;

        protected float _mouseX;
        protected float _mouseY;

        protected bool _mouseLookAllowed = true;

        protected float _cameraTopClamp;
        protected float _cameraBottomClamp;

        protected virtual void Start()
        {
            _mouseCursor.OnMouseCursorStateChanged?.AddListener(OnMouseCursorStateChanged);
            SetCameraClampAngles();
        }

        protected virtual void Update()
        {
            UpdateMouseLook();
        }

        protected virtual void OnDestroy()
        {
            _mouseCursor.OnMouseCursorStateChanged?.RemoveListener(OnMouseCursorStateChanged);
        }

        protected abstract void SetCameraClampAngles();

        protected abstract void ApplyPlayerRotation();

        protected virtual void UpdateMouseLook()
        {
            if (!_mouseLookAllowed)
            {
                return;
            }

            GetInput();
            ClampCameraRotation();
            ApplyPlayerRotation();
        }

        private void GetInput()
        {
            _mouseX = Input.GetAxis(MouseXInputAxis) * MouseInputMultiplier * _mouseYSensitivity * Time.deltaTime;
            _mouseY = Input.GetAxis(MouseYInputAxis) * MouseInputMultiplier * _mouseXSensitivity * Time.deltaTime;
        }

        private void ClampCameraRotation()
        {
            _cameraXRotation -= _mouseY;
            _cameraXRotation = Mathf.Clamp(_cameraXRotation, _cameraTopClamp, _cameraBottomClamp);
        }

        private void OnMouseCursorStateChanged(bool cursorIsLocked)
        {
            _mouseLookAllowed = cursorIsLocked;
        }
    }
}