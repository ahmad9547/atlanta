using UnityEngine;

namespace Metaverse.Services
{
    public sealed class LocalPlayerHolder : ILocalPlayerService
    {
        private const string PlayerTag = "Player";

        public GameObject LocalPlayer
        {
            get
            {
                if (_localPlayer == null)
                {
                    Debug.LogError("You try to access Local Player GameObject but it's reference is null");
                    return null;
                }

                return _localPlayer;
            }
            set
            {                
                _localPlayer = value;
            }
        }

        private GameObject _localPlayer;


        public bool IsOtherPlayerEqualsLocalPlayer(GameObject otherPlayer)
        {
            return otherPlayer.CompareTag(PlayerTag) && 
                otherPlayer == LocalPlayer;
        }
    }
}
