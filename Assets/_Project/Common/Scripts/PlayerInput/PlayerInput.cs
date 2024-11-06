using Common.PlayerInput.Interfaces;
using Core.ServiceLocator;
using UnityEngine;

namespace Common.PlayerInput
{
    public sealed class PlayerInput : MonoBehaviour
    {

        private const KeyCode SideMenuKeyCode = KeyCode.M;
        //private const KeyCode SpeakerAnimationKeyCode = KeyCode.P;
        private const KeyCode PresentationSlideLeftKeyCode = KeyCode.LeftArrow;
        private const KeyCode PresentationSlideRightKeyCode = KeyCode.RightArrow;
        private const KeyCode PresentationVideoPlayPauseKeyCode = KeyCode.UpArrow;
        private const KeyCode PresentationVideoResetKeyCode = KeyCode.DownArrow;
        //private const KeyCode CameraViewSwitchKeyCode = KeyCode.V;
        private const KeyCode InteractionKeyCode = KeyCode.E;

        #region Services

        private IPlayerInputEventHandler _playerInputEventHandlerInstance;
        private IPlayerInputEventHandler _playerInputEventHandler
            => _playerInputEventHandlerInstance ??= Service.Instance.Get<IPlayerInputEventHandler>();

        #endregion

        private void Update()
        {
            GetSideMenuInput();
            GetSpeakerAnimationInput();
            GetPresentationInput();
            GetCameraViewSwitchInput();
            GetInteractionInput();
        }

        private void GetSideMenuInput()
        {
            if (Input.GetKeyDown(SideMenuKeyCode))
            {
                _playerInputEventHandler.SwitchSideMenu();
            }
        }

        private void GetSpeakerAnimationInput()
        {
          /*  if (Input.GetKeyDown(SpeakerAnimationKeyCode))
            {
                _playerInputEventHandler.PlaySpeakerAnimation();
            }*/
        }

        private void GetPresentationInput()
        {
            if (Input.GetKeyDown(PresentationSlideLeftKeyCode))
            {
                _playerInputEventHandler.SwipePresentationSlideLeft();
            }

            if (Input.GetKeyDown(PresentationSlideRightKeyCode))
            {
                _playerInputEventHandler.SwipePresentationSlideRight();
            }

            if (Input.GetKeyDown(PresentationVideoPlayPauseKeyCode))
            {
                _playerInputEventHandler.PlayPausePresentationVideo();
            }

            if (Input.GetKeyDown(PresentationVideoResetKeyCode))
            {
                _playerInputEventHandler.ResetPresentationVideo();
            }
        }

        private void GetCameraViewSwitchInput()
        {
           /* if (Input.GetKeyDown(CameraViewSwitchKeyCode))
            {
                _playerInputEventHandler.SwitchCameraView();
            }*/
        }

        private void GetInteractionInput()
        {
            if (Input.GetKeyDown(InteractionKeyCode))
            {
                _playerInputEventHandler.Interact();
            }
        }
    }
}
