namespace PhotonEngine.PhotonEvents.Enums
{
    public enum PhotonEventCode : byte
    {
        KickEventCode = 0,
        PersonalMuteEventCode = 1,
        PersonalUnmuteEventCode = 2,
        PresentationSlideSwitch = 3,
        PresentationVideoPlay = 4,
        PresentationVideoPause = 5,
        PresentationVideoReset = 6,
        SeatPlaceOccupiedEventCode = 7,
        SeatPlaceFreeEventCode = 8,
        GlobalMuteEventCode = 9,
        GlobalUnmuteEventCode = 10
    }
}
