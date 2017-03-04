using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Sitecore.ChatBot.Utils;

namespace Sitecore.ChatBot.Services
{
	public partial class AppInsightsService
	{
		public static Task<string> GetNumberOfLogEntries(string severity, TimeSpan? timePeriod = null, string period = null)
		{
			string logLevel = $"customMetrics/Sitecore.System\\Logging | {severity} Logged / sec";
			period = period ?? (timePeriod != null ? DateConversionUtil.ToInsightsTimespan(timePeriod.Value) : null);
			return GetMetricValue(logLevel, period);
		}

		public static Task<string> GetNumberOfLogEntries(string severity,string period = null)
		{
			string logLevel = $"customMetrics/Sitecore.System\\Logging | {severity} Logged / sec";
			return GetMetricValue(logLevel, period);
		}
	}
}