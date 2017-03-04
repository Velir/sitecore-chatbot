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
        public partial class LuisIntentDialog : LuisDialog<object>
        {

            [LuisIntent("Failed Request Exceptions Query")]
            public async Task FailedRequestExceptionQuery(IDialogContext context, LuisResult result)
            {
                var entity = result.Entities.FirstOrDefault();

                var queryResult =  await AppInsightsService.GetFailedRequestExceptions(10, null);
                await context.PostAsync(queryResult);
                context.Wait(MessageReceived);
            }
        }
}