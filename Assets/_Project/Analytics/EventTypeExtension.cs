using System;
using System.Reflection;

namespace Metaverse.Analytics
{
    internal static class EventTypeExtension
    {
        public static string ParseEventName<TEventType>(this TEventType type)
            where TEventType : Enum
        {
            FieldInfo fieldInfo = typeof(TEventType).GetField(Enum.GetName(typeof(TEventType), type));

            OverrideEventNameAttribute overrideEventNameAttribute = (OverrideEventNameAttribute)
                Attribute.GetCustomAttribute(fieldInfo, typeof(OverrideEventNameAttribute));

            return overrideEventNameAttribute?.Name ?? type.ToString();
        }
    }
}