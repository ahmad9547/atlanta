using _Project.PlayerUIScene.Scripts.SideMenu.Services;
using Avatars.PersonMovement.Services;
using Avatars.Services;
using Avatars.WebGLMovement.MouseControll.Interfaces;
using Common.PlayerInput.Interfaces;
using Common.PlayerInput.Services;
using Core.FilesReading;
using Core.JwtManagement.Services;
using Core.ServiceLocator;
using Core.UrlParametersParsing;
using LoadingScreenScene;
using LocationsManagement.Interfaces;
using LocationsManagement.Services;
using Metaverse.Analytics.Services;
using Metaverse.AreaModification.Services;
using Metaverse.AreaModule.Services;
using Metaverse.Banners.Services;
using Metaverse.ErrorHandling.Services;
using Metaverse.GiftShop.Interfaces;
using Metaverse.GiftShop.Services;
using Metaverse.PlayersSettings;
using Metaverse.PresentationModule;
using Metaverse.PresetModification.Interfaces;
using Metaverse.PresetModification.Services;
using Metaverse.Services;
using Metaverse.Teleport.Database;
using Metaverse.Teleport.Interfaces;
using PhotonEngine.CustomProperties;
using PhotonEngine.CustomProperties.Services;
using PhotonEngine.Disconnection.Services;
using PhotonEngine.PhotonEvents;
using PhotonEngine.PhotonPlayers;
using PhotonEngine.PhotonRoom;
using PlayerUIScene.Services;
using PlayerUIScene.SideMenu.Mute;
using ProjectConfig.Session.Services;
using StartScene.Services;
using UnityEngine;
using UserManagement;
using VoiceChat;
using VoiceChat.Player;
using VoiceChat.WebGL;

namespace Persistent
{
    public static class PersistentServicesBinder
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RegisterServices()
        {
            Service serviceInstance = Service.Instance;

            serviceInstance.Register<INetworkService>(new PhotonEngine.PhotonRoom.Network());
            serviceInstance.Register<ILocationsHolderService>(new LocationsHolder());
            serviceInstance.Register<ILoadingScreenService>(new LoadingScreen());
            serviceInstance.Register<ILocationLoaderService>(new LocationsLoader());
            serviceInstance.Register<IAddressablesLoaderService>(new AddressablesLoader());
            serviceInstance.Register<IPlayerMovementLockerService>(new PlayerMovementLocker());
            serviceInstance.Register<ILocalPlayerService>(new LocalPlayerHolder());
            serviceInstance.Register<IDisconnectionCollector>(new DisconnectionCollector());
            serviceInstance.Register<IWebRequestsLoaderService>(new WebRequestsLoader());
            serviceInstance.Register<IStartupPointHolderService>(new StartupPointHolder());
            serviceInstance.Register<IPurchaseItemHolderService>(new PurchaseItemHolder());
            serviceInstance.Register<ICartItemHolderService>(new CartItemHolder());
            serviceInstance.Register<IMouseCursorService>(new MouseCursor());
            serviceInstance.Register<IPresetModificationService>(new PresetModificator());
            serviceInstance.Register<IRoomCustomPropertiesService>(new RoomCustomProperties());
            serviceInstance.Register<IAreaInformationHolderService>(new AreaInformationHolder());
            serviceInstance.Register<IPlayerSettingsService>(new PlayerSettings());
            serviceInstance.Register<IAreaModificator>(new AreaModificator());
            serviceInstance.Register<IPlayerInputEventHandler>(new PlayerInputEventHandler());
            serviceInstance.Register<IMenuService>(new MenuService());
            serviceInstance.Register<IMusicPlayerEventHandler>(new MusicPlayerEventHandler());
            serviceInstance.Register<IUserProfileService>(new UserProfile());
            serviceInstance.Register<IPlayerCustomPropertiesService>(new PlayerCustomProperties());
            serviceInstance.Register<IVoiceChatService>(new VoiceChat.VoiceChat());
            serviceInstance.Register<IWebVoiceChatAPIService>(new WebVoiceChatAPI());
            serviceInstance.Register<IMicrophoneStateService>(new MicrophoneState());
            serviceInstance.Register<IUserListService>(new UserList());
            serviceInstance.Register<IPhotonRoomPlayersService>(new PhotonRoomPlayers());
            serviceInstance.Register<IPlayerCreatorService>(new PlayerCreator());
            serviceInstance.Register<IPhotonEventsReceiverService>(new PhotonEventsReceiver());
            serviceInstance.Register<IPhotonEventsSenderService>(new PhotonEventsSender());
            serviceInstance.Register<ISpeakerZoneCheckerService>(new SpeakerZoneChecker());
            serviceInstance.Register<IPresentationSlidesService>(new PresentationSlides());
            serviceInstance.Register<IPresentationVideoPlayerService>(new PresentationVideoPlayer());
            serviceInstance.Register<ISpatialVoiceManagerService>(new SpatialVoiceManager());
            serviceInstance.Register<ISideMenuService>(new SideMenu());
            serviceInstance.Register<IPresentationVideoSyncService>(new PresentationVideoSync());
            serviceInstance.Register<IPersonalAdminMuteService>(new PersonalAdminMute());
            serviceInstance.Register<IGlobalAdminMuteService>(new GlobalAdminMute());
            serviceInstance.Register<ITeleportPointNamesService>(new TeleportPointNames());
            serviceInstance.Register<ITeleportControllerService>(new TeleportController());
            serviceInstance.Register<ICheckoutFrameCreatorService>(new CheckoutFrameCreator());
            serviceInstance.Register<IBannerContentLoaderService>(new BannerContentLoader());
            serviceInstance.Register<IWelcomeScreenStateHolderService>(new WelcomeScreenStateHolder());
            serviceInstance.Register<IAvatarAnimatorService>(new AvatarAnimatorHandler());
            serviceInstance.Register<IUrlParameterParserService>(new UrlParameterParser());
            serviceInstance.Register<IJwtDecoderService>(new JwtDecoder());
            serviceInstance.Register<ISessionConfigHandler>(new SessionConfigHandler());
            serviceInstance.Register<IStartSceneStateHolderService>(new StartSceneStateHolder());
            serviceInstance.Register<IAvatarLoadingStatusHolderService>(new AvatarLoadingStatusHolder());

#if !ANALYTICS_ENABLED
            serviceInstance.Register<IAnalyticsService>(new WarningAnalyticsService());
#elif UNITY_WEBGL
            serviceInstance.Register<IAnalyticsService>(new WebGLUnityAnalyticsService());
#else
            serviceInstance.Register<IAnalyticsService>(new EditorAnalyticsService());
#endif

            serviceInstance.Register<IErrorHandlingService>(new ErrorHandlingService());
        }
    }
}