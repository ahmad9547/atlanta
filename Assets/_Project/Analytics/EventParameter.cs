namespace Metaverse.Analytics
{
    internal sealed class EventParameter<T> : IEventParameter
    {
        public string Key { get; }
        public object Value { get; }

        public EventParameter(string key, T value)
        {
            Key = key;
            Value = value;
        }
    }
}