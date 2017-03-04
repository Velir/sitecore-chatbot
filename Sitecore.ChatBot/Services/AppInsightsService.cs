using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.Insights;
using Microsoft.Bot.Connector;
using Microsoft.Rest;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using Sitecore.ChatBot.Constants;

namespace Sitecore.ChatBot.Services
{
    public class AppInsightsService
    {
        public static async Task<string> GetNumberOfRequestsToServer()
        {
            var apiKey = ConfigurationManager.AppSettings[AppConstants.AppInsightsApiKey];
            var appId = ConfigurationManager.AppSettings[AppConstants.AppInsightsIdKey];
            //var creds = new MicrosoftAppCredentials()
            //{
            //    UserName = apiKey
            //};

            //using (var client = new InsightsClient(creds))
            //{
            //    client.SubscriptionId = "10616ee3-2788-4e6d-857b-91adf3a7e6a0";
            //    client.id

            //    var result = await client.Metrics.ListAsync("requests/count");

            //    return result.FirstOrDefault()?.Data?.FirstOrDefault()?.Count.ToString() ?? "Unknown Value";
            //}

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                var req = string.Format(AppConstants.AppInsightsEndpoint, appId, "metrics", "requests/count");
                var response = await client.GetAsync(req);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject<dynamic>(result);

                    return json.value["requests/count"].sum;
                }

                return null;
            }

        }
    }
}