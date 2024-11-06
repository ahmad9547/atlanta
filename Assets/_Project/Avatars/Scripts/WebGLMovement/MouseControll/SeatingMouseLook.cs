using UnityEngine;

namespace Avatars.WebGLMovement.MouseControll
{
    public sealed class SeatingMouseLook : MouseLook
    {
        private const float CameraTopClamp = -5f;
        private const float CameraBottomClamp = 60f;

        protected override void SetCameraClampAngles()
        {
            _cameraTopClamp = CameraTopClamp;
            _cameraBottomClamp = CameraBottomClamp;
        }

        protected override void ApplyPlayerRotation()
        {
            _cameraRoot.Rotate(Vector3.up * _mouseX);
            _cameraRoot.localRotation = Quaternion.Euler(_cameraXRotation, _cameraRoot.localRotation.eulerAngles.y, 0f);
        }
    }
}
