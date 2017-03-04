using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Sitecore.ChatBot.Utils;

namespace Sitecore.ChatBot.Services
{
    public partial class AppInsightsService
    {
        public static Task<string> GetFailedRequestExceptions(string timeInterval = "24h", int count = 10)
        {

            string query = Uri.EscapeDataString($"requests | where timestamp > ago({timeInterval}) and success== \"False\"| join kind = inner(exceptions | where timestamp > ago({timeInterval})) on operation_Id | project  type, problemId, requestName = name, requestDuration = duration | reduce by problemId | limit {count}");


            return GetQueryValue(query, json => PrettyPrintFailedRequestExceptions(json));
        }

        public static Task<string> GetFailedRequestExceptions(DateTime startDateTime, int count = 10)
        {

            string query = Uri.EscapeDataString($"requests | where timestamp > datetime({startDateTime.ToString("yyyy-MM-dd")}) and timestamp < datetime({startDateTime.ToString("yyyy-MM-dd")}) + 1d  and success== \"False\"| join kind = inner(exceptions | where timestamp > datetime({startDateTime.ToString("yyyy-MM-dd")}) and timestamp < datetime({startDateTime.ToString("yyyy-MM-dd")}) + 1d ) on operation_Id | project  type, problemId, requestName = name, requestDuration = duration | reduce by problemId | limit {count}");
      
            return GetQueryValue(query, json => PrettyPrintFailedRequestExceptions(json));
        }

        private static string PrettyPrintFailedRequestExceptions(dynamic json)
        {
            var rows = json.Tables[0].Rows;

            var builder = new StringBuilder("# (Count): Exception");
            builder.AppendLine();

            int numRows = 0;
            foreach (var row in rows)
            {
                var stackFrame = row[2];
                var count = row[1];

                builder.AppendLine($"* **({count})**: {stackFrame}");
                builder.AppendLine();

                numRows++;
            }

            return numRows == 0 
                ? "No results." 
                : builder.ToString();
        }
    }
}