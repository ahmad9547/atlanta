using System;

namespace Metaverse.Analytics.Services
{
    internal abstract class AnalyticsServiceBase : IAnalyticsService
    {
        public EventLogResult LogEvent<TEventType>(TEventType type, IEventParameter[] parameters)
            where TEventType : Enum
        {
            string eventName = type.ParseEventName();
            return LogEvent(eventName, parameters);
        }

        protected abstract EventLogResult LogEvent(string eventName, IEventParameter[] parameters);
    }
}