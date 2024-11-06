using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace PhotonEngine.CustomProperties.Services
{
    /// <summary>
    /// Custom properties are always merged. New keys are added, old values are updated. If value is null, key is removed.
    /// </summary>
    public sealed class RoomCustomProperties : IRoomCustomPropertiesService
    {
        private Hashtable _customPropertiesHashtable = new Hashtable();

        public void AddOrUpdateRoomCustomProperty(string key, object value)
        {
            _customPropertiesHashtable[key] = value.ToString();

            PhotonNetwork.CurrentRoom.SetCustomProperties(_customPropertiesHashtable);
        }

        public void AddOrUpdateRoomCustomProperty(string key)
        {
            _customPropertiesHashtable[key] = string.Empty;

            PhotonNetwork.CurrentRoom.SetCustomProperties(_customPropertiesHashtable);
        }

        public void RemoveRoomCustomProperty(string key)
        {
            _customPropertiesHashtable[key] = null;

            PhotonNetwork.CurrentRoom.SetCustomProperties(_customPropertiesHashtable);
        }

        public object GetRoomCustomProperty(string key)
        {
            Hashtable customProperties = PhotonNetwork.CurrentRoom.CustomProperties;

            return customProperties.TryGetValue(key, out object value) ? value : null;
        }
        
        public bool CustomPropertiesContainKey(string key)
        {
            return PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(key);
        }
    }
}
