using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Sitecore.ChatBot.Services;

namespace Sitecore.ChatBot.Dialogs
{
    [LuisModel("eb2abdfb-ade9-4dbf-96f7-3eda7b7596a6", "095cef5a0a844442a009402eff0436ff")]
    [Serializable]
    public partial class LuisIntentDialog : LuisDialog<object>
    {
        private const string TimePeriodEntity = "builtin.datetime.duration";

        [LuisIntent("View Total Requests")]
        public async Task ViewTotalRequests(IDialogContext context, LuisResult result)
        {
            var entity = result.Entities.FirstOrDefault();

            if (entity != null && entity.Type == TimePeriodEntity)
            {
                TimeSpan span;
                TimeSpan? timeSpan = TimeSpan.TryParse(entity.Entity, out span) ? (TimeSpan?) span : null;
                var count = await AppInsightsService.GetNumberOfRequestsToServer(timeSpan, entity.Resolution.FirstOrDefault().Value);

                var timeString = timeSpan == null ? entity.Resolution.FirstOrDefault().Value : timeSpan.ToString();

                await context.PostAsync($"There have been {count} requests to the server in the requested timeframe ({timeString})");
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