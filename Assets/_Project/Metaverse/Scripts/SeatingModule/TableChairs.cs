using Core.UI;
using System.Collections.Generic;
using System.Linq;
using Avatars.Enums;
using Avatars.Services;
using Core.ServiceLocator;
using UnityEngine;
using UnityEngine.UI;

namespace Metaverse.SeatingModule
{
    public sealed class TableChairs : SeatingObject
    {
        [SerializeField] private List<SeatingPlace> _seatingPlaces;
        [SerializeField] private TableChairsInfoPopup _tableChairsInfoPopup;
        [SerializeField] private AnimatorOverrideControllerType _overrideControllerType;

        [Header("Info popup settings")] [SerializeField]
        private Image _tableFreeImage;

        [SerializeField] private Image _tableOccupiedImage;

        private const float DefaultClosestChairDistance = 1000f;

        private UIAnimator _uiAnimator = new UIAnimator();

        private SeatingPlace _occupiedSeatingPlace;

        #region Services

        private IAvatarAnimatorService _avatarAnimatorInstance;
        private IAvatarAnimatorService _avatarAnimatorHandler
            => _avatarAnimatorInstance ??= Service.Instance.Get<IAvatarAnimatorService>();

        #endregion

        public override void StartInteractionButtonClick()
        {
            if (_occupiedSeatingPlace != null || _isSeatingObjectInteractionTimerActive)
            {
                return;
            }

            if (GetAnyClosestFreeSeatingPlaceNearTable(out SeatingPlace freeSeatingPlace))
            {
                SeatDownOnChair(freeSeatingPlace);
                HideInfoWindows();
            }
        }

        public override void EndInteractionButtonClick()
        {
            if (_occupiedSeatingPlace == null || _isSeatingObjectInteractionTimerActive)
            {
                return;
            }

            StandUpFromChair();
            ShowInfoWindow();
        }

        public override void OnInteractionZoneTriggerEnter(GameObject player)
        {
            base.OnInteractionZoneTriggerEnter(player);

            _avatarAnimatorHandler.OverrideAnimatorController(_overrideControllerType);
            _tableChairsInfoPopup.SetTargetFacingObject(player);
        }

        public override void OnInteractionZoneTriggerExit(GameObject player)
        {
            base.OnInteractionZoneTriggerExit(player);

            _tableChairsInfoPopup.RemoveTargetFacingObject();

            if (_occupiedSeatingPlace == null || _isSeatingObjectInteractionTimerActive)
            {
                return;
            }

            StandUpFromChair();
        }

        protected override void ShowInfoWindow()
        {
            bool isFree = AnyFreeSeatingPlaceNearTable();
            SetTableInfoImagesState(isFree);
            _uiAnimator.ShowWindow(isFree ? _tableFreeImage.transform : _tableOccupiedImage.transform);
        }

        protected override void HideInfoWindows()
        {
            _uiAnimator.HideWindow(_tableFreeImage.transform, () =>
            {
                _tableFreeImage.gameObject.SetActive(false);
            });

            _uiAnimator.HideWindow(_tableOccupiedImage.transform, () =>
            {
                _tableOccupiedImage.gameObject.SetActive(false);
            });
        }

        private void SeatDownOnChair(SeatingPlace freeSeatingPlace)
        {
            freeSeatingPlace.OccupyPlace();
            _occupiedSeatingPlace = freeSeatingPlace;
            ActivateInteractionTimer();
            _playerSeating?.SeatDownOnTableChair(freeSeatingPlace);
            _seatingCamera.gameObject.SetActive(true);
        }

        private void StandUpFromChair()
        {
            _occupiedSeatingPlace.UnoccupyPlace();
            _playerSeating?.StandUpFromTableChair(_occupiedSeatingPlace.StandingUpPosition);
            _occupiedSeatingPlace = null;
            ActivateInteractionTimer();
            _seatingCamera.gameObject.SetActive(false);
        }

        private bool GetAnyClosestFreeSeatingPlaceNearTable(out SeatingPlace freeSeatingPlace)
        {
            Vector3 playerPosition = _playerSeating.transform.position;

            freeSeatingPlace = null;
            float closestDistance = DefaultClosestChairDistance;

            foreach (SeatingPlace seatingPlace in _seatingPlaces)
            {
                if (!seatingPlace.IsFree)
                {
                    continue;
                }

                float distance = (playerPosition - seatingPlace.Position).sqrMagnitude;

                if (distance <= closestDistance)
                {
                    freeSeatingPlace = seatingPlace;
                    closestDistance = distance;
                }
            }

            return freeSeatingPlace != null;
        }

        private bool AnyFreeSeatingPlaceNearTable()
        {
            return _seatingPlaces.Any(seatingPlace => seatingPlace.IsFree);
        }

        private void SetTableInfoImagesState(bool isFree)
        {
            _tableOccupiedImage.gameObject.SetActive(!isFree);
            _tableFreeImage.gameObject.SetActive(isFree);
        }
    }
}
