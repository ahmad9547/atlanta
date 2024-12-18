using Avatars.WebGLMovement.MouseControll;
using Avatars.PersonMovement;
using Metaverse.SeatingModule;
using Photon.Pun;
using System;
using System.Collections;
using Core.ServiceLocator;
using Metaverse.Teleport.Interfaces;
using UnityEngine;

namespace Avatars.WebGLMovement
{
    [RequireComponent(typeof(PhotonView))]
    public sealed class PlayerSeating : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerMouseLook _playerMouseLook;
        [SerializeField] private CameraSwitcher _cameraSwitcher;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private GameObject _playersDistanceColliders;

        private const float PlayerStandingTimeInSeconds = 0.7f;
        private const float CameraBlendDelayWhenTeleportedWithMap = 1f;

        private PhotonView _photonView;

        private Coroutine _waitWhilePlayerStandingCoroutine;

        private bool _isPlayerSeating = false;

        #region Services

        private ITeleportControllerService _mapTeleportControllerInstance;
        private ITeleportControllerService _mapTeleportController
            => _mapTeleportControllerInstance ??= Service.Instance.Get<ITeleportControllerService>();

        #endregion

        private void Awake()
        {
            _photonView = gameObject.GetComponent<PhotonView>();
        }

        private void OnEnable()
        {
            _mapTeleportController.TeleportedWithMapEvent.AddListener(OnTeleportedWithMap);
        }

        private void OnDisable()
        {
            _mapTeleportController.TeleportedWithMapEvent.RemoveListener(OnTeleportedWithMap);
        }

        public void SeatDownOnBench(SeatingPlace seatingPlace)
        {
            if (!_photonView.IsMine)
            {
                return;
            }

            SeatDown(seatingPlace.Position, seatingPlace.transform.rotation);
            _playerAnimator.SetSeatingOnBenchParameter(true);
        }

        public void StandUpFromBench(Vector3 standUpPosition)
        {
            if (!_photonView.IsMine)
            {
                return;
            }

            _playerAnimator.SetSeatingOnBenchParameter(false);

            if (_waitWhilePlayerStandingCoroutine != null)
            {
                StopCoroutine(WaitWhilePlayerStanding());
            }

            _waitWhilePlayerStandingCoroutine = StartCoroutine(WaitWhilePlayerStanding(() =>
            {
                gameObject.transform.SetPositionAndRotation(standUpPosition, transform.rotation);
            }));
        }

        public void SeatDownOnTableChair(SeatingPlace seatingPlace)
        {
            if (!_photonView.IsMine)
            {
                return;
            }

            SeatDown(seatingPlace.Position, seatingPlace.transform.rotation);
            _playerAnimator.SetSeatingOnTableChairParameter(true);
        }

        public void StandUpFromTableChair(Vector3 standUpPosition)
        {
            if (!_photonView.IsMine)
            {
                return;
            }

            _playerAnimator.SetSeatingOnTableChairParameter(false);
            SetupPlayerSeatingStates(false);
            gameObject.transform.SetPositionAndRotation(standUpPosition, transform.rotation);
        }

        public void SeatDownOnAdminChair(SeatingPlace seatingPlace)
        {
            if (!_photonView.IsMine)
            {
                return;
            }

            SeatDown(seatingPlace.Position, seatingPlace.transform.rotation);
            _playerAnimator.SetSeatingOnAdminChairParameter(true);
        }

        public void StandUpFromAdminChair(Vector3 standUpPosition)
        {
            if (!_photonView.IsMine)
            {
                return;
            }

            _playerAnimator.SetSeatingOnAdminChairParameter(false);
            SetupPlayerSeatingStates(false);
            gameObject.transform.SetPositionAndRotation(standUpPosition, transform.rotation);
        }

        public void OnSeatingZoneEnter()
        {
            _playersDistanceColliders.SetActive(false);
        }

        public void OnSeatingZoneExit()
        {
            _playersDistanceColliders.SetActive(true);
        }

        private IEnumerator WaitWhilePlayerStanding(Action callback = null)
        {
            yield return new WaitForSeconds(PlayerStandingTimeInSeconds);
            SetupPlayerSeatingStates(false);
            callback?.Invoke();
        }

        private void SeatDown(Vector3 position, Quaternion rotation)
        {
            SetupPlayerSeatingStates(true);
            gameObject.transform.SetPositionAndRotation(position, rotation);
        }

        private void SetupPlayerSeatingStates(bool isPlayerSeating)
        {
            _playerMovement.FreezePlayerPhysics(!isPlayerSeating);
            _cameraSwitcher.TogglePlayerControllCameras(isPlayerSeating);
            _playerMouseLook.enabled = !isPlayerSeating;
            _isPlayerSeating = isPlayerSeating;
            _playerMovement.IsPlayerSeating = isPlayerSeating;
        }

        private void OnTeleportedWithMap()
        {
            if (!_isPlayerSeating)
            {
                return;
            }

            _cameraSwitcher.ToggleCamerasBlend(false);
            SetupPlayerSeatingStates(false);
            _playerAnimator.ResetAnimator();
            _cameraSwitcher.ToggleCamerasBlend(true, CameraBlendDelayWhenTeleportedWithMap);
        }
    }
}