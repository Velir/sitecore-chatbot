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
        [LuisIntent("View Failed Requests")]
        public async Task ViewFailedRequests(IDialogContext context, LuisResult result)
        {
            var entity = result.Entities.FirstOrDefault(x => x.Type.StartsWith(TimePeriodEntityPrefix));

            if (entity != null && entity.Type.StartsWith(TimePeriodEntityPrefix))
            {
                string customPeriod = entity.Resolution.FirstOrDefault().Value;

                DateTime dateTime;
                if (DateTime.TryParse(customPeriod, out dateTime))
                {
                    // A date was specified, not a duration, so transform to a time period
                    customPeriod = $"{customPeriod}/{(dateTime + TimeSpan.FromDays(1)):yyyy-MM-dd}";
                }

                var count = await AppInsightsService.GetNumberOfFailedRequests(customPeriod);

                if (string.IsNullOrWhiteSpace(count))
                {
                    count = "0";
                }

                await context.PostAsync(
                    $"There have been {count} failed requests to the server in the requested timeframe ({customPeriod})");
                context.Wait(MessageReceived);
            }
            else
            {
                var count = await AppInsightsService.GetNumberOfFailedRequests();

                await context.PostAsync($"There have been a total of {count} failed requests to the server.");
                context.Wait(MessageReceived);
            }

        }
    }
}