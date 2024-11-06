using Core.ServiceLocator;
using Metaverse.InteractionModule.Interfaces;
using Metaverse.Services;
using System.Collections.Generic;
using Common.PlayerInput.Interfaces;
using UnityEngine;

namespace Metaverse.InteractionModule
{
    public sealed class InteractionZoneHandler : MonoBehaviour
    {
        [Header("Debug:")] [SerializeField] private Color _interactionZoneColor;

        public bool IsInteractionAllowed { get; set; } = true;

        private readonly List<IInteractionZoneTrigger> _interactionZoneTriggerObservables =
            new List<IInteractionZoneTrigger>();

        private readonly List<IInteractionZoneButton> _interactionZoneButtonObservables =
            new List<IInteractionZoneButton>();

        #region Services

        private ILocalPlayerService _localPlayerHolderInstance;
        private ILocalPlayerService _localPlayerHolder
            => _localPlayerHolderInstance ??= Service.Instance.Get<ILocalPlayerService>();

        private IPlayerInputEventHandler _playerInputEventHandlerInstance;
        private IPlayerInputEventHandler _playerInputEventHandler
            => _playerInputEventHandlerInstance ??= Service.Instance.Get<IPlayerInputEventHandler>();

        #endregion


        private bool _playerInZone;
        private bool _playerInteracting;

        private void OnTriggerEnter(Collider other)
        {
            if (!IsLocalPlayer(other.gameObject) || _playerInZone)
            {
                return;
            }

            _playerInZone = true;

            _interactionZoneTriggerObservables.ForEach(observable => observable.OnInteractionZoneTriggerEnter(other.gameObject));
            _playerInputEventHandler.OnInteraction.AddListener(ToggleInteraction);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsLocalPlayer(other.gameObject) || !_playerInZone)
            {
                return;
            }

            _playerInZone = false;

            _interactionZoneTriggerObservables.ForEach(observable => observable.OnInteractionZoneTriggerExit(other.gameObject));
            _playerInputEventHandler.OnInteraction.RemoveListener(ToggleInteraction);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _interactionZoneColor;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }

        public void AddInteractionZoneButtonObserver(IInteractionZoneButton observer)
        {
            if (_interactionZoneButtonObservables.Contains(observer))
            {
                Debug.LogError("Interaction zone button observer already added");
                return;
            }

            _interactionZoneButtonObservables.Add(observer);
        }

        public void RemoveInteractionZoneButtonObserver(IInteractionZoneButton observer)
        {
            if (!_interactionZoneButtonObservables.Contains(observer))
            {
                Debug.LogError("Interaction zone button observer not found");
                return;
            }

            _interactionZoneButtonObservables.Remove(observer);
        }

        public void AddInteractionZoneTriggerObserver(IInteractionZoneTrigger observer)
        {
            if (_interactionZoneTriggerObservables.Contains(observer))
            {
                Debug.LogError("Interaction zone trigger observer already added");
                return;
            }

            _interactionZoneTriggerObservables.Add(observer);
        }

        public void RemoveInteractionZoneTriggerObserver(IInteractionZoneTrigger observer)
        {
            if (!_interactionZoneTriggerObservables.Contains(observer))
            {
                Debug.LogError("Interaction zone trigger observer not found");
                return;
            }

            _interactionZoneTriggerObservables.Remove(observer);
        }

        private bool IsLocalPlayer(GameObject other)
        {
            return _localPlayerHolder.IsOtherPlayerEqualsLocalPlayer(other);
        }

        private void ToggleInteraction()
        {
            if (!IsInteractionAllowed)
            {
                return;
            }

            _interactionZoneButtonObservables.ForEach(_playerInteracting
                ? interactable => interactable.EndInteractionButtonClick()
                : interactable => interactable.StartInteractionButtonClick());

            _playerInteracting = !_playerInteracting;
        }
    }
}