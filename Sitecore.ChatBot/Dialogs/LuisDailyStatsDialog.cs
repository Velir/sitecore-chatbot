using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Sitecore.ChatBot.Services;

namespace Sitecore.ChatBot.Dialogs
{
	[LuisModel("eb2abdfb-ade9-4dbf-96f7-3eda7b7596a6", "095cef5a0a844442a009402eff0436ff")]
	public partial class LuisIntentDialog : LuisDialog<object>
	{


		[LuisIntent("View Daily Stats")]
		public async Task ViewDailyStats(IDialogContext context, LuisResult result)
		{

			var results = await AppInsightsService.GetDailyStats(null);

			await context.PostAsync($"Your daily stats are: ");
			await context.PostAsync($"{results}");
			context.Wait(MessageReceived);
		}


	}
}