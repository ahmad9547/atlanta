using Photon.Pun;
using UnityEngine;

namespace Avatars.WebGLMovement.MouseControll
{
    public sealed class PlayerMouseLook : MouseLook
    {
        [SerializeField] private Transform _playerBody;
        [SerializeField] private PhotonView _photonView;

        private const float FirstPersonCameraTopClamp = -60f;
        private const float FirstPersonCameraBottomClamp = 60f;

        private const float ThirdPersonCameraTopClamp = -5f;
        private const float ThirdPersonCameraBottomClamp = 60f;       

        public void SetFirstPersonCameraViewAngle()
        {
            _cameraTopClamp = FirstPersonCameraTopClamp;
            _cameraBottomClamp = FirstPersonCameraBottomClamp;
        }

        public void SetThirdPersonCameraViewAngle()
        {
            _cameraTopClamp = ThirdPersonCameraTopClamp;
            _cameraBottomClamp = ThirdPersonCameraBottomClamp;
        }

        protected override void SetCameraClampAngles()
        {
            SetFirstPersonCameraViewAngle();
        }

        protected override void UpdateMouseLook()
        {
            if (!_photonView.IsMine)
            {
                return;
            }

            base.UpdateMouseLook();
        }

        protected override void ApplyPlayerRotation()
        {
            _cameraRoot.localRotation = Quaternion.Euler(_cameraXRotation, 0f, 0f);
            _playerBody.Rotate(Vector3.up * _mouseX);
        }
    }
}
