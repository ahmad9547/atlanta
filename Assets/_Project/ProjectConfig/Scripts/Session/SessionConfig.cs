using Core.ServiceLocator;
using Metaverse.ErrorHandling;
using Metaverse.ErrorHandling.Services;
using ProjectConfig.Session.Services;

namespace ProjectConfig.Session
{
    public static class SessionConfig
    {
        private const string DefaultNickname = "player";
        private const string DefaultRoomName = "DevelopmentRoom";

        private static string _nickname;
        private static string _avatarLink;
        private static bool _adminStatus;
        private static string _locationId;
        private static string _teleportPointId;
        private static string _roomId;

        #region Services

        private static ISessionConfigHandler _sessionConfigHandlerService;
        private static ISessionConfigHandler _sessionConfigHandler
            => _sessionConfigHandlerService ??= Service.Instance.Get<ISessionConfigHandler>();

        #endregion

        public static string Nickname
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_nickname))
                {
                    _sessionConfigHandler.AddErrorMessage("Nickname is empty, default nickname is being set.");
                    return DefaultNickname;
                }

                return _nickname;
            }
        }

        public static string AvatarLink => _avatarLink;
        public static bool AdminStatus => _adminStatus;
        public static string LocationId => _locationId;
        public static string TeleportPointId => _teleportPointId;

        public static string RoomId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_roomId))
                {
                    _sessionConfigHandler.AddErrorMessage("Room ID is empty, default room name is being set.");
                    return DefaultRoomName;
                }

                return _roomId;
            }
        }

        public static void SetupSessionConfig(SessionConfigModel sessionConfigModel)
        {
            _nickname = sessionConfigModel.nickname;
            _avatarLink = sessionConfigModel.avatarId;
            _adminStatus = sessionConfigModel.adminStatus;
            _locationId = sessionConfigModel.locationId;
            _teleportPointId = sessionConfigModel.teleportPointId;
            _roomId = sessionConfigModel.roomId;
        }
    }
}