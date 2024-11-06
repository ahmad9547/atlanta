using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Analytics;
using Unity.Services.Core;

namespace Metaverse.Analytics.Services
{
    internal sealed class WebGLUnityAnalyticsService : AnalyticsServiceBase
    {
        public WebGLUnityAnalyticsService()
        {
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
        }

        protected override EventLogResult LogEvent(string eventName, IEventParameter[] parameters)
        {
            if (parameters is null)
            {
                try
                {
                    AnalyticsService.Instance.CustomData(eventName);
                    return EventLogResult.Success;
                }
                catch (Exception)
                {
                    return EventLogResult.Fail;
                }
            }

            Dictionary<string, object> analyticsParameters = new Dictionary<string, object>();

            foreach (IEventParameter parameter in parameters)
            {
                analyticsParameters.Add(parameter.Key, parameter.Value);
            }

            try
            {
                AnalyticsService.Instance.CustomData(eventName, analyticsParameters);
                return EventLogResult.Success;
            }
            catch (Exception)
            {
                return EventLogResult.Fail;
            }
        }
    }
}