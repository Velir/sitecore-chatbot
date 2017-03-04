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
			string logLevel = $"customMetrics%2FSitecore.System%5CLogging%20%7C%20{severity}%20Logged%20%2F%20sec?aggregation=unique";
			period = period ?? (timePeriod != null ? DateConversionUtil.ToInsightsTimespan(timePeriod.Value) : null);
			return GetMetricValue(logLevel, period, json => json.value[$"customMetrics/Sitecore.System\\Logging | {severity} Logged / sec"].unique);
		}

		public static Task<string> GetNumberOfLogEntries(string severity,string period = null)
		{
			string logLevel = $"customMetrics%2FSitecore.System%5CLogging%20%7C%20{severity}%20Logged%20%2F%20sec?aggregation=unique";
			return GetMetricValue(logLevel, period, json => json.value[$"customMetrics/Sitecore.System\\Logging | {severity} Logged / sec"].unique);
		}

       
      
	}
}