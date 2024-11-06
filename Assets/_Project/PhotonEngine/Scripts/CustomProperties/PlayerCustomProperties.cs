using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using PhotonEngine.CustomProperties.Enums;
using Photon.Realtime;

namespace PhotonEngine.CustomProperties
{
    /// <summary>
    /// Custom properties are always merged. New keys are added, old values are updated. If value is null, key is removed.
    /// </summary>
    public sealed class PlayerCustomProperties : IPlayerCustomPropertiesService
    {
        private Hashtable _customPropertiesHashtable = new Hashtable();

        public void AddOrUpdateLocalPlayerCustomProperty(PlayerCustomPropertyKey key, object value)
        {
            _customPropertiesHashtable[key.ToString()] = value.ToString();

            PhotonNetwork.LocalPlayer.SetCustomProperties(_customPropertiesHashtable);
        }

        public void AddOrUpdateLocalPlayerCustomProperty(PlayerCustomPropertyKey key)
        {
            _customPropertiesHashtable[key.ToString()] = string.Empty;

            PhotonNetwork.LocalPlayer.SetCustomProperties(_customPropertiesHashtable);
        }

        public void RemoveLocalPlayerCustomProperty(PlayerCustomPropertyKey key)
        {
            _customPropertiesHashtable[key.ToString()] = null;

            PhotonNetwork.LocalPlayer.SetCustomProperties(_customPropertiesHashtable);
        }

        public object GetSpecialPlayerCustomProperty(Player player, PlayerCustomPropertyKey key)
        {
            Hashtable customProperties = player.CustomProperties;

            return customProperties.TryGetValue(key.ToString(), out object value) ? value : null;
        }

        public bool IsPlayerHaveCustomProperty(Player player, PlayerCustomPropertyKey key, out object propertyValue)
        {
            Hashtable customProperties = player.CustomProperties;

            propertyValue = customProperties.TryGetValue(key.ToString(), out object value) ? value : null;

            return customProperties.ContainsKey(key.ToString());
        }
    }
}