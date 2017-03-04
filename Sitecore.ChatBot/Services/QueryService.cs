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
            string query = "requests%20%20%7C%20where%20timestamp%20%3E%20ago(24h)%20and%20success%3D%3D%22False%22%20%7C%20join%20kind%3D%20inner%20(exceptions%20%20%7C%20where%20timestamp%20%3E%20ago(24h)%20)%20on%20operation_Id%20%20%7C%20project%20%20type%2C%20method%2C%20requestName%20%3D%20name%2C%20requestDuration%20%3D%20duration";
            return GetQueryResult(query);
        }
    }
}