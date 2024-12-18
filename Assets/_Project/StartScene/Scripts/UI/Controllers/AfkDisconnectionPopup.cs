using PhotonEngine.Disconnection;
using UnityEngine;

namespace StartScene.UI.Controllers
{
    public sealed class AfkDisconnectionPopup : DisconnectionPopup
    {
        // leaving empty start to prevent Hide() call from inherited class
        protected override void Start() { }

        protected override void OnDisconnectionInfoUpdated(DisconnectionMessage disconnectionMessage)
        {
            if (disconnectionMessage.DisconnectionType != DisconnectionType.AfkDisconnection)
            {
                Hide();
                return;
            }

            Show();
        }
    }
}
