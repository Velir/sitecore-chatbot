using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sitecore.ChatBot.Constants;
using Sitecore.ChatBot.Utils;

namespace Sitecore.ChatBot.Services
{
    public class AppInsightsService
    {
        public static Task<string> GetNumberOfRequestsToServer(TimeSpan? timePeriod)
        {
            const string metric = "requests/count";
            return GetMetricValue(metric, timePeriod);
        }

        public static Task<string> GetNumberOfFailedRequests(TimeSpan? timePeriod)
        {
            const string metric = "requests/failed";
            return GetMetricValue(metric, timePeriod);
        }

        private static async Task<string> GetMetricValue(string metricName, TimeSpan? timePeriod)
        {
            using (var client = CreateClient())
            {
                var url = CreateMetricUri(metricName);
                url = timePeriod == null
                    ? url
                    : $"{url}?timespan={DateConversionUtil.ToInsightsTimespan(timePeriod.Value)}";

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject<dynamic>(result);

                    return json.value[metricName].sum;
                }

                return null;
            }
        }

        private static string CreateMetricUri(string metricName)
        {
            var appId = ConfigurationManager.AppSettings[AppConstants.AppInsightsIdKey];
            return string.Format(AppConstants.AppInsightsEndpoint, appId, "metrics", metricName);
        }

        private static HttpClient CreateClient()
        {
            var apiKey = ConfigurationManager.AppSettings[AppConstants.AppInsightsApiKey];
            var appId = ConfigurationManager.AppSettings[AppConstants.AppInsightsIdKey];

            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);

            return client;
        }
    }
}

