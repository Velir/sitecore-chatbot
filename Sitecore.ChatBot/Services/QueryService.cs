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
        public static Task<string> GetFailedRequestExceptions(int count = 10, TimeSpan? timeperiod = null)
        {

            string query = Uri.EscapeDataString("requests | where timestamp > ago(7d) and success== \"False\"| join kind = inner(exceptions | where timestamp > ago(24h)) on operation_Id | project  type, method, requestName = name, requestDuration = duration| limit 10");
            return GetQueryResult(query);
        }
    }
}