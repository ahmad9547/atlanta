using Photon.Realtime;

namespace PhotonEngine.Disconnection
{
    public sealed class PhotonDisconnectionMessage : DisconnectionMessage
    {
        public string Message { get; private set; }

        public PhotonDisconnectionMessage(DisconnectCause cause)
        {
            DisconnectionType = DisconnectionType.PhotonErrorDisconnection;

            Message = cause.ToString();
        }
    }
}
