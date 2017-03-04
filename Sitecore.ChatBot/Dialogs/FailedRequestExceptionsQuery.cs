using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Sitecore.ChatBot.Services;
using Sitecore.ChatBot.Utils;

namespace Sitecore.ChatBot.Dialogs
{
    [LuisModel("eb2abdfb-ade9-4dbf-96f7-3eda7b7596a6", "095cef5a0a844442a009402eff0436ff")]
    public partial class LuisIntentDialog : LuisDialog<object>
    {

        [LuisIntent("Failed Request Exceptions Query")]
        public async Task FailedRequestExceptionQuery(IDialogContext context, LuisResult result)
        {
            //Retrieve Count if it exists
            int count = 10;
            EntityRecommendation entity = result.Entities.FirstOrDefault(x => x.Type == "builtin.number");
            if (entity != null)
            {
                int.TryParse(entity.Entity, out count);
            }

            string query = string.Empty;

            var dateEntity = result.Entities.FirstOrDefault(x => x.Type.StartsWith(TimePeriodEntityPrefix));
            if (dateEntity != null)
            {
                switch (dateEntity.Type)
                {
                    case "builtin.datetime.duration":
                        string queryTimeInterval = DateConversionUtil.ToInsightsQueryTimeInterval(dateEntity.Resolution.FirstOrDefault().Value);
                        query = await AppInsightsService.GetFailedRequestExceptions(queryTimeInterval, count);
                        break;
                    case "builtin.datetime.date":
                        string startDateTime = dateEntity.Resolution.FirstOrDefault().Value;
                        DateTime dateTime;
                        if (!DateTime.TryParse(startDateTime, out dateTime))
                        {
                            goto default;
                        }
                        query = await AppInsightsService.GetFailedRequestExceptions(dateTime, count);
                        break;
                    default:
                        query = await AppInsightsService.GetFailedRequestExceptions(null, count);
                        break;
                }
            }
           
            await context.PostAsync(query);
            context.Wait(MessageReceived);
        }

    }
}
