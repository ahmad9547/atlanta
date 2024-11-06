namespace Metaverse.Analytics
{
    internal interface IEventParameter
    {
        string Key { get; }
        object Value { get; }
    }
}