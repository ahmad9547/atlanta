using System.Text;
using UnityEngine;

namespace Metaverse.Analytics.Services
{
    internal class EditorAnalyticsService : AnalyticsServiceBase
    {
        protected override EventLogResult LogEvent(string eventName, IEventParameter[] parameters)
        {
            if (parameters is null)
            {
                Debug.Log($"Event logged {eventName} without parameters");
                return EventLogResult.Success;
            }

            StringBuilder parametersBuilder = new StringBuilder();

            foreach (IEventParameter parameter in parameters)
            {
                parametersBuilder.Append($"[{parameter.Key}: {parameter.Value}],");
            }

            parametersBuilder.Remove(parametersBuilder.Length - 1, 1);

            Debug.Log($"Event logged {eventName} with parameters: {parametersBuilder}");

            return EventLogResult.Success;
        }
    }
}