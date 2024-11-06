using UnityEngine;
using Avatars.Player;
using Common.PlayerInput.Interfaces;
using Core.ServiceLocator;

namespace Metaverse.PresentationModule
{
    public sealed class SpeakerAnimator : MonoBehaviour
    {
        private PlayerComponentsReferences _playerComponentsReferences;

        private bool _isSpeakerAnimated = false;

        #region Services

        private IPlayerInputEventHandler _playerInputEventHandlerInstance;
        private IPlayerInputEventHandler _playerInputEventHandler
            => _playerInputEventHandlerInstance ??= Service.Instance.Get<IPlayerInputEventHandler>();

        #endregion

        private void OnDestroy()
        {
            _playerInputEventHandler.OnPlaySpeakerAnimation?.RemoveListener(OnSpeakerAnimationInput);
        }

        public void SetupSpeakerAnimatorMode()
        {
            _playerInputEventHandler.OnPlaySpeakerAnimation?.AddListener(OnSpeakerAnimationInput);

            _playerComponentsReferences = gameObject.GetComponent<PlayerComponentsReferences>();
            if (_playerComponentsReferences == null)
            {
                Debug.LogError("Player has no PlayerComponentsReferences component");
            }
        }

        public void StopSpeakerAnimatorMode()
        {
            _playerInputEventHandler.OnPlaySpeakerAnimation?.RemoveListener(OnSpeakerAnimationInput);

            if (_isSpeakerAnimated)
            {
                SetSpeakerAnimatedState(false);
            }
        }

        private void OnSpeakerAnimationInput()
        {
            if (_playerComponentsReferences == null)
            {
                Debug.LogError("Player has no PlayerComponentsReferences component");
                return;
            }

            if (!_playerComponentsReferences.PlayerMovement.IsGrounded)
            {
                return;
            }

            _isSpeakerAnimated = !_isSpeakerAnimated;
            SetSpeakerAnimatedState(_isSpeakerAnimated);
        }

        private void SetSpeakerAnimatedState(bool isSpeakerAnimated)
        {
            _playerComponentsReferences.PlayerMovement.FreezePlayerPhysics(!isSpeakerAnimated);
            _playerComponentsReferences.PlayerAnimator.SetSpeakerParameter(isSpeakerAnimated);

            if (!isSpeakerAnimated)
            {
                _playerComponentsReferences.PlayerAnimator.ResetAnimator();
            }
        }
    }
}
