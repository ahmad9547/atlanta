namespace Metaverse.Analytics
{
    internal enum PhotonConnectionEventType
    {
        [OverrideEventName("connectedToServer")]
        ConnectedToServer,
        [OverrideEventName("joinedLobby")]
        JoinedLobby,
        [OverrideEventName("joinedRoom")]
        JoinedRoom,
        [OverrideEventName("disconnectedFromServer")]
        DisconnectedFromServer,
    }
}