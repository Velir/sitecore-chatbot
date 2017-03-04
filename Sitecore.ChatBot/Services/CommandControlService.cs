using System;
using System.Configuration;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Sitecore.ChatBot.Constants;

namespace Sitecore.ChatBot.Services
{
    public class CommandControlService
    {
        private static TimeSpan Timeout = TimeSpan.FromSeconds(5);

        protected static string AuthKey => ConfigurationManager.AppSettings[AppConstants.CCServerAuthKey];
        protected static string Endpoint => ConfigurationManager.AppSettings[AppConstants.CCServerEndpointKey];

        public static async Task<string> GetCurrentLogLevel()
        {
            const string methodName = "GetCurrentLogLevel";

            using (var client = new HttpClient())
            {
                client.Timeout = Timeout;
                var uri = $"{Endpoint}/{methodName}?authKey={AuthKey}";
                var response = await client.GetStringAsync(uri);

                return HackAwayXml(response);
            }
        }

        public static async Task<string> SetLogLevel(string level)
        {
            const string methodName = "SetLogLevel";

            using (var client = new HttpClient())
            {
                client.Timeout = Timeout;
                var uri = $"{Endpoint}/{methodName}?authKey={AuthKey}&logSeverity={level}";
                return HackAwayXml(await client.GetStringAsync(uri));
            }
        }

        private static string HackAwayXml(string response)
        {
            return response
                .Replace(@"<string xmlns=""http://tempuri.org/"">", string.Empty)
                .Replace(@"</string>", string.Empty)
                .Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r", string.Empty);
        }
    }
}