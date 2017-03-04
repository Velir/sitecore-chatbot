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
        public static Task<string> GetFailedRequestExceptions(string timeInterval = "24h", int count = 10)
        {

            string query = Uri.EscapeDataString($"requests | where timestamp > ago({timeInterval}) and success== \"False\"| join kind = inner(exceptions | where timestamp > ago({timeInterval})) on operation_Id | project  type, method, requestName = name, requestDuration = duration| limit {count}");


            return GetQueryValue(query, json => json.ToString());
        }

        public static Task<string> GetFailedRequestExceptions(DateTime startDateTime, int count = 10)
        {

            string query = Uri.EscapeDataString($"requests | where timestamp > datetime({startDateTime.ToString("yyyy-MM-dd")}) and timestamp < datetime({startDateTime.ToString("yyyy-MM-dd")}) + 1d  and success== \"False\"| join kind = inner(exceptions | where timestamp > datetime({startDateTime.ToString("yyyy-MM-dd")}) and timestamp < datetime({startDateTime.ToString("yyyy-MM-dd")}) + 1d ) on operation_Id | project  type, method, requestName = name, requestDuration = duration| limit {count}");
      
            return GetQueryValue(query, json => json.ToString());
        }
    }
}