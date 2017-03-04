using System;
using System.Text;

namespace Sitecore.ChatBot.Utils
{
    public class DateConversionUtil
    {
        public static string ToInsightsTimespan(TimeSpan period)
        {
            // Converts a time span to a string for viewing the past N days/hours
            var builder = new StringBuilder("P");

            if (period.Days > 0) builder.AppendFormat("{0}D", period.Days);
            if (period.Hours > 0) builder.AppendFormat("{0}H", period.Hours);

            return builder.ToString();
        }

        public static string ToInsightsQueryTimeInterval(string period)
        {
            //Convert the provided period to a time interval that is compatible with App Insights query 
            //syntax, specifically the ago() operator
            period = period.ToLower();
            period = period.Replace("pt", "");

            return period;
        }
    }
}