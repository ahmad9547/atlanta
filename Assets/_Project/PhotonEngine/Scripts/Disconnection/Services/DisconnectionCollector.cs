namespace PhotonEngine.Disconnection.Services
{
    public sealed class DisconnectionCollector : IDisconnectionCollector
    {
        public bool HasDisconnectionInfo { get; private set; }
        public DisconnectionMessage DisconnectionMessage { get; private set; }


        public void SetDisconnectionMessage(DisconnectionMessage message)
        {
            DisconnectionMessage = message;

            HasDisconnectionInfo = true;
        }

        public void ResetDisconnectionInfo()
        {
            DisconnectionMessage = null;

            HasDisconnectionInfo = false;
        }
    }
}
