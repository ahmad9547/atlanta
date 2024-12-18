using Core.ServiceLocator;
using Photon.Realtime;
using PhotonEngine.CustomProperties.Enums;

namespace PhotonEngine.CustomProperties
{
    public interface IPlayerCustomPropertiesService : IService
    {
        void AddOrUpdateLocalPlayerCustomProperty(PlayerCustomPropertyKey key);
        void AddOrUpdateLocalPlayerCustomProperty(PlayerCustomPropertyKey key, object value);
        void RemoveLocalPlayerCustomProperty(PlayerCustomPropertyKey key);
        object GetSpecialPlayerCustomProperty(Player player, PlayerCustomPropertyKey key);
        bool IsPlayerHaveCustomProperty(Player player, PlayerCustomPropertyKey key, out object propertyValue);
    }
}