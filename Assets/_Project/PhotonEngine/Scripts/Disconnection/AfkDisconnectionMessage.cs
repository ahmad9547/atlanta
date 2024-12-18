namespace PhotonEngine.Disconnection
{
    public sealed class AfkDisconnectionMessage : DisconnectionMessage
    {
        public AfkDisconnectionMessage()
        {
            DisconnectionType = DisconnectionType.AfkDisconnection;
        }
    }
}
