using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace Sitecore.ChatBot.Dialogs
{
    public partial class LuisIntentDialog
    {
        private static readonly string[] ExampleCommands =
        {
            "my daily stats",
            "the total number of requests to the server",
            "the number of failed requests",
            "the number of requests for the past hour",
            "the error log count",
            "the warning log count",
            "my current log level"
        };

        

        [LuisIntent("Greetings")]
        public async Task GreetUser(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Hi there! My name is Sitecore ChatOps!  Why don't you ask me a question?");

            var builder = new StringBuilder("Try asking 'Show me '...");
            builder.AppendLine();

            var rand = new Random();
            foreach (var suggestion in ExampleCommands.OrderBy(k => rand.Next()).Take(3))
            {
                builder.AppendLine($"* {suggestion}");
            }

            await context.PostAsync(builder.ToString());
            context.Wait(MessageReceived);
        }

    }
}