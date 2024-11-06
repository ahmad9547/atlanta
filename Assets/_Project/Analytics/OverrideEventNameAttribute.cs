using System;

namespace Metaverse.Analytics
{
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class OverrideEventNameAttribute : Attribute
    {
        public string Name { get; }

        public OverrideEventNameAttribute(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid name provided, cannot be null, empty or whitespaces");
            }

            Name = name;
        }
    }
}