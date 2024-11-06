using System;

namespace ProjectConfig.Session
{
    [Serializable]
    public class SessionConfigModel
    {
        public string nickname;
        public string avatarId;
        public bool adminStatus;
        public string locationId;
        public string teleportPointId;
        public string roomId;
    }
}