using UnityEngine;

namespace Avatars.Player
{
    public sealed class PlayerFacingTopUI : MonoBehaviour
    {
        [SerializeField] private Transform _topUIContent;

        private Camera _mainCamera;

        private void Start()
        {
            SetMainCamera();
        }

        private void Update()
        {
            RotateTopUIToMainCamera();
        }

        private void RotateTopUIToMainCamera()
        {
            if (_mainCamera == null)
            {
                SetMainCamera();
            }

            Vector3 rotation = Quaternion.LookRotation(_topUIContent.position - _mainCamera.transform.position).eulerAngles;
            rotation.x = 0f;
            rotation.z = 0f;
            _topUIContent.rotation = Quaternion.Euler(rotation);
        }

        private void SetMainCamera()
        {
            _mainCamera = Camera.main;
        }
    }
}
