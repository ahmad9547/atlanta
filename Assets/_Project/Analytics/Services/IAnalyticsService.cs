using System;
using Core.ServiceLocator;

namespace Metaverse.Analytics.Services
{
    internal interface IAnalyticsService : IService
    {
        EventLogResult LogEvent<TEventType>(TEventType type, IEventParameter[] parameters)
            where TEventType : Enum;
    }
}