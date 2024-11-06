using System;
using Core.ServiceLocator;
using Metaverse.Analytics.Services;

namespace Metaverse.Analytics
{
    internal static class EventLogger
    {
        private static IAnalyticsService _analyticsService;

        private static IAnalyticsService AnalyticsService =>
            _analyticsService ??= Service.Instance.Get<IAnalyticsService>();

        public static EventLogResult Log<TEventType>(TEventType type, params IEventParameter[] parameters)
            where TEventType : Enum
        {
#if ANALYTICS_ENABLED
            return AnalyticsService.LogEvent(type, parameters);
#endif
            return EventLogResult.Fail;
        }
    }
}