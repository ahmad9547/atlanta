using System;
using System.Collections;
using Avatars.WebGLMovement.MouseControll;
using Cinemachine;
using Common.PlayerInput.Interfaces;
using Core.ServiceLocator;
using Metaverse.PlayersSettings;
using UnityEngine;

namespace Avatars.WebGLMovement
{
    public sealed class CameraSwitcher : MonoBehaviour
    {
        private const float CamerasDisabledBlend = 0f;
        private const float CamerasEnabledBlend = 1f;

        [SerializeField] private GameObject _controllerCameras;
        [SerializeField] private CinemachineVirtualCamera _firstPersonCamera;
        [SerializeField] private CinemachineVirtualCamera _thirdPersonCamera;
        [SerializeField] private CinemachineBrain _cinemachineBrain;

        [SerializeField] private PlayerMouseLook _playerMouseLook;

        #region Services

        private IPlayerSettingsService _playerSettingsInstance;
        private IPlayerSettingsService _playerSettings
            => _playerSettingsInstance ??= Service.Instance.Get<IPlayerSettingsService>();

        private IPlayerInputEventHandler _playerInputEventHandlerInstance;
        private IPlayerInputEventHandler _playerInputEventHandler
            => _playerInputEventHandlerInstance ??= Service.Instance.Get<IPlayerInputEventHandler>();

        #endregion

        private bool _isFirstPersonCameraActive = false;

        private readonly Vector3 _thirdPersonCameraDefaultDamping = new Vector3(0.5f, 0f, 2f);

        private Cinemachine3rdPersonFollow _thirdPersonCameraTransposer;

        private Coroutine _camerasBlendToggleCoroutine;

        private void Awake()
        {
            _thirdPersonCameraTransposer = _thirdPersonCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }

        private void OnEnable()
        {
            _playerInputEventHandler.OnCameraViewSwitch.AddListener(OnCameraSwitchInput);
            _playerSettings.OnThirdPersonCameraDefaultDampingSet.AddListener(SetThirdPersonCameraDefaultDamping);
            _playerSettings.OnThirdPersonCameraDampingDisabled.AddListener(DisableThirdPersonCameraDamping);
        }

        private void Start()
        {
            ToggleViewCamera(_playerSettings.IsThirdPersonCameraActive);
        }

        private void OnDisable()
        {
            _playerInputEventHandler.OnCameraViewSwitch.RemoveListener(OnCameraSwitchInput);
            _playerSettings.OnThirdPersonCameraDefaultDampingSet.RemoveListener(SetThirdPersonCameraDefaultDamping);
            _playerSettings.OnThirdPersonCameraDampingDisabled.RemoveListener(DisableThirdPersonCameraDamping);
        }

        public void TogglePlayerControllCameras(bool arePlayerControllCamerasActive)
        {
            _controllerCameras.SetActive(!arePlayerControllCamerasActive);
        }

        private void DisableThirdPersonCameraDamping()
        {
            _thirdPersonCameraTransposer.Damping = Vector3.zero;
        }

        private void SetThirdPersonCameraDefaultDamping()
        {
            _thirdPersonCameraTransposer.Damping = _thirdPersonCameraDefaultDamping;
        }

        public void ToggleCamerasBlend(bool isBlendActive, float delay = 0f)
        {
            if (delay == 0)
            {
                SetCamerasBlendState(isBlendActive);
                return;
            }

            if (_camerasBlendToggleCoroutine != null)
            {
                StopCoroutine(_camerasBlendToggleCoroutine);
                _camerasBlendToggleCoroutine = null;
            }

            _camerasBlendToggleCoroutine = StartCoroutine(CamerasBlendToggleDelay(isBlendActive, delay));
        }

        private void OnCameraSwitchInput()
        {
            _isFirstPersonCameraActive = !_isFirstPersonCameraActive;
            ToggleViewCamera(_isFirstPersonCameraActive);
        }

        private void ToggleViewCamera(bool isFirstPersonActive)
        {
            _firstPersonCamera.gameObject.SetActive(!isFirstPersonActive);
            _thirdPersonCamera.gameObject.SetActive(isFirstPersonActive);
            _playerSettings.IsThirdPersonCameraActive = isFirstPersonActive;

            (isFirstPersonActive
                ? (Action)_playerMouseLook.SetThirdPersonCameraViewAngle
                : _playerMouseLook.SetFirstPersonCameraViewAngle).Invoke();
        }

        private IEnumerator CamerasBlendToggleDelay(bool isBlendActive, float delay)
        {
            yield return new WaitForSeconds(delay);
            SetCamerasBlendState(isBlendActive);
            _camerasBlendToggleCoroutine = null;
        }

        private void SetCamerasBlendState(bool isBlendActive)
        {
            _cinemachineBrain.m_DefaultBlend.m_Time = isBlendActive ? CamerasEnabledBlend : CamerasDisabledBlend;
        }
    }
}