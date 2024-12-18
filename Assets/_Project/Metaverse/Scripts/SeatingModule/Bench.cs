using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Core.UI;

namespace Metaverse.SeatingModule
{
    public sealed class Bench : SeatingObject
    {
        [SerializeField] private List<SeatingPlace> _seatingPlaces;

        [Header("Info popup settings")]
        [SerializeField] private Image _benchFreeImage;
        [SerializeField] private Image _benchOccupiedImage;

        private UIAnimator _uiAnimator = new UIAnimator();

        private SeatingPlace _occupiedSeatingPlace;

        public override void StartInteractionButtonClick()
        {
            if (_occupiedSeatingPlace != null || _isSeatingObjectInteractionTimerActive)
            {
                return;
            }

            if (GetAnyFreeSeatingPlaceOnBench(out SeatingPlace freeSeatingPlace))
            {
                SeatDownOnBench(freeSeatingPlace);
                HideInfoWindows();
            }
        }

        public override void EndInteractionButtonClick()
        {
            if (_occupiedSeatingPlace == null || _isSeatingObjectInteractionTimerActive)
            {
                return;
            }

            StandUpFromBench();
            ShowInfoWindow();
        }

        public override void OnInteractionZoneTriggerExit(GameObject player)
        {
            base.OnInteractionZoneTriggerExit(player);

            if (_occupiedSeatingPlace == null || _isSeatingObjectInteractionTimerActive)
            {
                return;
            }

            StandUpFromBench();
        }

        protected override void ShowInfoWindow()
        {
            bool isFree = AnyFreeSeatingPlaceOnBench();
            SetBenchInfoImagesState(isFree);
            _uiAnimator.ShowWindow(isFree ? _benchFreeImage.transform : _benchOccupiedImage.transform);
        }

        protected override void HideInfoWindows()
        {
            _uiAnimator.HideWindow(_benchFreeImage.transform, () =>
            {
                _benchFreeImage.gameObject.SetActive(false);
            });

            _uiAnimator.HideWindow(_benchOccupiedImage.transform, () =>
            {
                _benchOccupiedImage.gameObject.SetActive(false);
            });
        }

        private void SeatDownOnBench(SeatingPlace freeSeatingPlace)
        {
            freeSeatingPlace.OccupyPlace();
            _occupiedSeatingPlace = freeSeatingPlace;
            ActivateInteractionTimer();
            _playerSeating?.SeatDownOnBench(freeSeatingPlace);
            _seatingCamera.gameObject.SetActive(true);
        }

        private void StandUpFromBench()
        {
            _occupiedSeatingPlace.UnoccupyPlace();
            _playerSeating?.StandUpFromBench(_occupiedSeatingPlace.StandingUpPosition);
            _occupiedSeatingPlace = null;
            ActivateInteractionTimer();
            _seatingCamera.gameObject.SetActive(false);
        }

        private bool GetAnyFreeSeatingPlaceOnBench(out SeatingPlace freeSeatingPlace)
        {
            freeSeatingPlace = _seatingPlaces.FirstOrDefault(seatingPlace => seatingPlace.IsFree);
            return freeSeatingPlace != null;
        }

        private bool AnyFreeSeatingPlaceOnBench()
        {
            return _seatingPlaces.Any(seatingPlace => seatingPlace.IsFree);
        }

        private void SetBenchInfoImagesState(bool isFree)
        {
            _benchOccupiedImage.gameObject.SetActive(!isFree);
            _benchFreeImage.gameObject.SetActive(isFree);
        }
    }
}
