namespace Metaverse.Analytics
{
    internal enum ErrorEventType
    {
        [OverrideEventName("errorOccured")]
        Error,
        [OverrideEventName("exceptionOccured")]
        Exception,
        [OverrideEventName("assertOccured")]
        Assert,
    }
}