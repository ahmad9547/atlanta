using Core.ServiceLocator;

namespace PhotonEngine.CustomProperties.Services
{
    public interface IRoomCustomPropertiesService : IService
    {
        void AddOrUpdateRoomCustomProperty(string key, object value);

        void AddOrUpdateRoomCustomProperty(string key);

        void RemoveRoomCustomProperty(string key);

        object GetRoomCustomProperty(string key);

        bool CustomPropertiesContainKey(string key);
    }
}