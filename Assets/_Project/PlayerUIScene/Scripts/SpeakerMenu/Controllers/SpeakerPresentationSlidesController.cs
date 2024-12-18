using Core.UI;
using Common.PlayerInput.Interfaces;
using Core.ServiceLocator;
using Metaverse.PresentationModule.Interfaces;
using Metaverse.PresentationModule;

namespace PlayerUIScene.SpeakerMenu.Controllers
{
    public sealed class SpeakerPresentationSlidesController : UIController, ISpeakerZoneObserver
    {
        #region Services

        private IPlayerInputEventHandler _playerInputEventHandlerInstance;
        private IPlayerInputEventHandler _playerInputEventHandler
            => _playerInputEventHandlerInstance ??= Service.Instance.Get<IPlayerInputEventHandler>();

        private IPresentationSlidesService _presentationSlidesInstance;
        private IPresentationSlidesService _presentationSlides
            => _presentationSlidesInstance ??= Service.Instance.Get<IPresentationSlidesService>();

        private ISpeakerZoneCheckerService _speakerZoneCheckerService;
        private ISpeakerZoneCheckerService _speakerZoneChecker
            => _speakerZoneCheckerService ??= Service.Instance.Get<ISpeakerZoneCheckerService>();

        #endregion

        private void OnEnable()
        {
            _speakerZoneChecker.AddSpeakerZoneObserver(this);
        }

        protected override void Start()
        {
            Show();
        }

        private void OnDisable()
        {
            _speakerZoneChecker.RemoveSpeakerZoneObserver(this);
        }

        public void SpeakerZoneEntered()
        {
            _playerInputEventHandler.OnSwipePresentationLeft?.AddListener(OnPresentationSlideLeftClick);
            _playerInputEventHandler.OnSwipePresentationRight?.AddListener(OnPresentationSlideRightClick);
        }

        public void SpeakerZoneLeft()
        {
            _playerInputEventHandler.OnSwipePresentationLeft?.RemoveListener(OnPresentationSlideLeftClick);
            _playerInputEventHandler.OnSwipePresentationRight?.RemoveListener(OnPresentationSlideRightClick);
        }

        private void OnPresentationSlideLeftClick()
        {
            _presentationSlides.SelectPreviousSlide();
        }

        private void OnPresentationSlideRightClick()
        {
            _presentationSlides.SelectNextSlide();
        }
    }
}