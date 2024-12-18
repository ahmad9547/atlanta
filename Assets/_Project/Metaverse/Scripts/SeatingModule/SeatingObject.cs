using Avatars.WebGLMovement;
using Avatars.WebGLMovement.MouseControll;
using Metaverse.InteractionModule;
using Metaverse.InteractionModule.Interfaces;
using UnityEngine;

namespace Metaverse.SeatingModule
{
    public class SeatingObject : MonoBehaviour, IInteractionZoneButton, IInteractionZoneTrigger
    {
        [SerializeField] protected InteractionZoneHandler _interactionZoneHandler;
        [SerializeField] protected SeatingMouseLook _seatingCamera;

        private const float SeatingObjectInteractionInterval = 3f;        

        protected bool _isSeatingObjectInteractionTimerActive = false;
        protected PlayerSeating _playerSeating;

        private float _seatingObjecthInteractionTimer;        

        protected virtual void OnEnable()
        {
            _interactionZoneHandler.AddInteractionZoneButtonObserver(this);
            _interactionZoneHandler.AddInteractionZoneTriggerObserver(this);
        }

        protected virtual void Update()
        {
            UpdateSeatingObjectInteractionTimer();
        }

        protected virtual void OnDisable()
        {
            _interactionZoneHandler.RemoveInteractionZoneButtonObserver(this);
            _interactionZoneHandler.RemoveInteractionZoneTriggerObserver(this);
        }       

        public virtual void StartInteractionButtonClick() { }

        public virtual void EndInteractionButtonClick() { }

        public virtual void OnInteractionZoneTriggerEnter(GameObject player)
        {
            ShowInfoWindow();           

            if (_playerSeating != null)
            {
                Debug.LogError("PlayerSeating component already set");
                return;
            }

            _playerSeating = player.GetComponent<PlayerSeating>();

            if (_playerSeating == null)
            {
                Debug.LogError("PlayerSeating component reference is null");
                return;
            }

            _playerSeating.OnSeatingZoneEnter();
        }

        public virtual void OnInteractionZoneTriggerExit(GameObject player)
        {
            HideInfoWindows();            

            if (_playerSeating == null)
            {
                Debug.LogError("PlayerSeating component is null");
                return;
            }

            _playerSeating.OnSeatingZoneExit();

            _playerSeating = null;
        }

        protected virtual void ShowInfoWindow() { }

        protected virtual void HideInfoWindows() { }

        protected void ActivateInteractionTimer()
        {
            _isSeatingObjectInteractionTimerActive = true;
            _interactionZoneHandler.IsInteractionAllowed = false;
        }        

        private void UpdateSeatingObjectInteractionTimer()
        {
            if (!_isSeatingObjectInteractionTimerActive)
            {
                return;
            }

            _seatingObjecthInteractionTimer += Time.deltaTime;

            if (_seatingObjecthInteractionTimer >= SeatingObjectInteractionInterval)
            {
                _isSeatingObjectInteractionTimerActive = false;
                _interactionZoneHandler.IsInteractionAllowed = true;
                _seatingObjecthInteractionTimer = 0f;
            }
        }
    }
}
