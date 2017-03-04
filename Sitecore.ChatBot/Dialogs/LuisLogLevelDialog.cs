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
        private const string LogLevelPrefix = "severity::";

        [LuisIntent("View Current Log Level")]
        public async Task ViewCurrentLogLevel(IDialogContext context, LuisResult result)
        {
            var currentLogLevel = await CommandControlService.GetCurrentLogLevel();

            await context.PostAsync($"Your Sitecore's log level is current set to {currentLogLevel}.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Set Log Level")]
        public async Task SetLogLevel(IDialogContext context, LuisResult result)
        {
            var level = result.Entities.FirstOrDefault(x => x.Type.StartsWith(LogLevelPrefix))?.Entity.Replace(LogLevelPrefix, string.Empty);

            var currentLogLevel = await CommandControlService.SetLogLevel(level);

            await context.PostAsync($"Your Sitecore's log level has been set to {currentLogLevel}.");
            context.Wait(MessageReceived);
        }
    }
}