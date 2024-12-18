using UnityEngine;

namespace Metaverse.Analytics.Services
{
    internal sealed class WarningAnalyticsService : AnalyticsServiceBase
    {
        protected override EventLogResult LogEvent(string eventName, IEventParameter[] parameters)
        {
            Debug.LogWarning($"You are trying to log event {eventName}, but analytics is disabled");
            return EventLogResult.Fail;
        }
    }
}