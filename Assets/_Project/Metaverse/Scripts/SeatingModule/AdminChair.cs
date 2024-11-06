using Core.ServiceLocator;
using Core.UI;
using UnityEngine;
using UnityEngine.UI;
using UserManagement;

namespace Metaverse.SeatingModule
{
    public sealed class AdminChair : SeatingObject
    {
        [SerializeField] private SeatingPlace _seatingPlace;

        [Header("Info popup settings")]
        [SerializeField] private Image _chairFreeImage;
        [SerializeField] private Image _chairOccupiedImage;

        private bool _isLocallyOccupied;
        private bool _isAdmin => _userProfile.IsAdmin;

        private UIAnimator _uiAnimator = new UIAnimator();

        #region Services

        private IUserProfileService _userProfileService;
        private IUserProfileService _userProfile
            => _userProfileService ??= Service.Instance.Get<IUserProfileService>();

        #endregion

        public override void StartInteractionButtonClick()
        {
            if (!_isAdmin || _isSeatingObjectInteractionTimerActive)
            {
                return;
            }

            if (_seatingPlace.IsFree)
            {
                SeatDownOnChair();
                HideInfoWindows();
            }
        }

        public override void EndInteractionButtonClick()
        {
            if (!_isAdmin || _isSeatingObjectInteractionTimerActive)
            {
                return;
            }

            if (_seatingPlace.IsFree)
            {
                return;
            }

            if (_isLocallyOccupied)
            {
                StandUpFromChair();
                ShowInfoWindow();
            }
        }

        public override void OnInteractionZoneTriggerEnter(GameObject player)
        {
            if (!_isAdmin)
            {
                return;
            }

            base.OnInteractionZoneTriggerEnter(player);
        }

        protected override void ShowInfoWindow()
        {
            if (!_isAdmin)
            {
                return;
            }

            bool isFree = _seatingPlace.IsFree;
            SetChairInfoImagesState(isFree);
            _uiAnimator.ShowWindow(isFree ? _chairFreeImage.transform : _chairOccupiedImage.transform);
        }

        protected override void HideInfoWindows()
        {
            if (!_isAdmin)
            {
                return;
            }

            _uiAnimator.HideWindow(_chairFreeImage.transform, () =>
            {
                _chairFreeImage.gameObject.SetActive(false);
            });

            _uiAnimator.HideWindow(_chairOccupiedImage.transform, () =>
            {
                _chairOccupiedImage.gameObject.SetActive(false);
            });
        }

        private void SeatDownOnChair()
        {
            _isLocallyOccupied = true;
            _seatingPlace.OccupyPlace();
            ActivateInteractionTimer();
            _playerSeating?.SeatDownOnAdminChair(_seatingPlace);
            _seatingCamera.gameObject.SetActive(true);
        }

        private void StandUpFromChair()
        {
            _seatingPlace.UnoccupyPlace();
            ActivateInteractionTimer();
            _playerSeating?.StandUpFromAdminChair(_seatingPlace.StandingUpPosition);
            _seatingCamera.gameObject.SetActive(false);
            _isLocallyOccupied = false;
        }

        private void SetChairInfoImagesState(bool isFree)
        {
            _chairOccupiedImage.gameObject.SetActive(!isFree);
            _chairFreeImage.gameObject.SetActive(isFree);
        }
    }
}