using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sitecore.ChatBot.Services
{
	public partial class AppInsightsService
	{

		public static Task<string> GetDailyStats(string severity, string period = null)
		{
			string query = $"exceptions%20%20%7C%20where%20timestamp%20%3E%20ago(24h)%20%20%7C%20count";
			return GetQueryValue(query, json => json);
		}
	}
}