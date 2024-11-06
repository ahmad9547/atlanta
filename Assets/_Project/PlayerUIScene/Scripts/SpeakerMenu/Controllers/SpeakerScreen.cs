using Core.ServiceLocator;
using Core.UI;
using LocationsManagement.Enums;
using LocationsManagement.Interfaces;
using Metaverse.PresentationModule;
using Metaverse.PresentationModule.Interfaces;

namespace PlayerUIScene.SpeakerMenu.Controllers
{
    public sealed class SpeakerScreen : UIController, ISpeakerZoneObserver
    {
        #region Services

        private ILocationLoaderService _locationLoaderInstance;
        private ILocationLoaderService _locationLoader
            => _locationLoaderInstance ??= Service.Instance.Get<ILocationLoaderService>();

        private ISpeakerZoneCheckerService _speakerZoneCheckerService;
        private ISpeakerZoneCheckerService _speakerZoneChecker
            => _speakerZoneCheckerService ??= Service.Instance.Get<ISpeakerZoneCheckerService>();

        #endregion

        private readonly LocationType _locationTypeWhenScreenIsVisible = LocationType.OlympicPark;

        private void OnEnable()
        {
            _speakerZoneChecker.AddSpeakerZoneObserver(this);
        }

        protected override void Start()
        {
            CheckIfScreenShouldBeVisible();
        }

        private void OnDisable()
        {
            _speakerZoneChecker.RemoveSpeakerZoneObserver(this);
        }

        public void SpeakerZoneEntered()
        {
            Show();
        }

        public void SpeakerZoneLeft()
        {
            Hide();
        }

        private void CheckIfScreenShouldBeVisible()
        {
            if (_locationLoader.LoadedLocation.LocationType != _locationTypeWhenScreenIsVisible)
            {
                Destroy(this.gameObject);
                return;
            }

            Hide();
        }
    }
}