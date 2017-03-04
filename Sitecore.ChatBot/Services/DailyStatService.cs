using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sitecore.ChatBot.Services
{
	public partial class AppInsightsService
	{

		public static Task<string> GetDailyExceptions()
		{
			string query = $"exceptions%20%20%7C%20where%20timestamp%20%3E%20ago(24h)%20%20%7C%20count";
			return GetQueryValue(query, json => json.Tables[0].Rows[0][0].ToString());
		}

        public static Task<string> GetDailyRequests()
        {
            string query = $"requests%20%20%7C%20where%20timestamp%20%3E%20ago(24h)%20%20%7C%20count";
            return GetQueryValue(query, json => json.Tables[0].Rows[0][0].ToString());
        }
   
        public static Task<string> GetDailyCacheClearings()
        {
            string query = "customMetrics%20%7C%20where%20timestamp%20%3E%20ago(24h)%20and%20name%20%3D%3D%20'Sitecore.Caching%5C%5CCache%20Clearings%20%2F%20sec'%20%7C%20count";
            return GetQueryValue(query, json => json.Tables[0].Rows[0][0].ToString());
        }
    }
}