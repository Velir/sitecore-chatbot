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

		[LuisIntent("View Log Count")]
		public async Task ViewLogCount(IDialogContext context, LuisResult result)
		{
			var entityDateTime = result.Entities.FirstOrDefault(x => x.Type == "datetime");
			var entitySeverity = result.Entities.FirstOrDefault(x => x.Type == "severity");
			var severity = ResolveSeverity(entitySeverity != null ? entitySeverity.Entity : string.Empty);

			if (entityDateTime != null && entityDateTime.Type == TimePeriodEntityPrefix)
			{
				TimeSpan span;
				TimeSpan? timeSpan = TimeSpan.TryParse(entityDateTime.Entity, out span) ? (TimeSpan?) span : null;
				


				var count = await AppInsightsService.GetNumberOfLogEntries(severity, timeSpan, entityDateTime.Resolution.FirstOrDefault().Value);

				var timeString = timeSpan == null ? entityDateTime.Resolution.FirstOrDefault().Value : timeSpan.ToString();

				await context.PostAsync($"There have been {count} {severity} log entries to the server in the requested timeframe ({timeString})");
				context.Wait(MessageReceived);
			}
			else
			{
				var count = await AppInsightsService.GetNumberOfLogEntries(severity, null);

				await context.PostAsync($"There have been a total of {count} {severity} log entries to the server.");
				context.Wait(MessageReceived);
			}
		}

		private static string ResolveSeverity(string luisEntityName)
		{
			switch (luisEntityName)
			{
				case "warning":
					return "Warnings";
				default:
					return string.Empty;
			}
		}

	}
}