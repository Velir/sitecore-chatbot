using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Sitecore.ChatBot.Services;

namespace Sitecore.ChatBot.Dialogs
{
    public partial class LuisIntentDialog
    {
        private const string ResourceEntityPrefix = "resource::";

        public string[] AvailableResources = { "Requests", "Exceptions" };


        public string CurrentResource { get; set; }

        [LuisIntent("Show Available Resources")]
        public async Task ShowAvailableResources(IDialogContext context, LuisResult result)
        {
            var resources = string.Join(", ", AvailableResources);

            await context.PostAsync($"Here are the available resources you may query on:\n {resources}");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Perform Resource Query")]
        public async Task QueryOnResource(IDialogContext context, LuisResult result)
        {
            var resourceName =
                result.Entities.FirstOrDefault(x => x.Type.StartsWith(ResourceEntityPrefix))?
                    .Entity.Replace(ResourceEntityPrefix, string.Empty);

            // Parse out (optional) time entity
            var timespanEntity = result.Entities.FirstOrDefault(x => x.Type.StartsWith(TimePeriodEntityPrefix));
            if (timespanEntity != null)
            {
                string customPeriod = timespanEntity.Resolution.FirstOrDefault().Value;

                DateTime dateTime;
                if (DateTime.TryParse(customPeriod, out dateTime))
                {
                    // A date was specified, not a duration, so transform to a time period
                    customPeriod = $"{customPeriod}/{(dateTime + TimeSpan.FromDays(1)):yyyy-MM-dd}";
                }

                var count = await AppInsightsService.GetNumberOfRequestsToServer(customPeriod);

                if (string.IsNullOrWhiteSpace(count))
                {
                    count = "0";
                }

                await context.PostAsync($"There have been {count} requests to the server in the requested timeframe ({customPeriod})");
                context.Wait(MessageReceived);
            }
            else
            {
                var count = await AppInsightsService.GetNumberOfRequestsToServer();

                await context.PostAsync($"There have been a total of {count} requests to the server.");
                context.Wait(MessageReceived);
            }
        }
    }
}