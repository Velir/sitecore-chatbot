using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using log4net;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using log4net.spi;
using Sitecore.Chatbot.Services.Utils;

namespace Sitecore.ChatBot.Services
{
    /// <summary>
    /// Summary description for CommandControlService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class CommandControlService : WebService
    {
        private const string AuthKeyAppSettingName = "Sitecore.Chatbot.CommandControl.AuthKey";

        // This REALLY shouldn't be a GET request, but oh well
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string SetLogLevel(string logSeverity, string authKey)
        {
            if (!AuthenticateRequest(authKey)) return null;

            Context.Response.ContentType = "application/json";

            var repository = LogManager.GetLoggerRepository() as Hierarchy;
            var level = ParseLevel(logSeverity);

            repository.Threshold = level;
            repository.Root.Level = level;

            dynamic wrapped = DynamicWrapper.For<LoggerRepositorySkeleton>(repository);
            wrapped.FireConfigurationResetEvent();

            var serializer = new JavaScriptSerializer();

            return serializer.Serialize(level.Name);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string GetCurrentLogLevel(string authKey)
        {
            if (!AuthenticateRequest(authKey)) return null;

            Context.Response.ContentType = "application/json";
            var serializer = new JavaScriptSerializer();

            return serializer.Serialize(((Hierarchy) LogManager.GetLoggerRepository()).Root.Level.Name);
        }

        private Level ParseLevel(string logSeverity)
        {
            switch (logSeverity.ToLowerInvariant())
            {
                case "warn":
                    return Level.WARN;
                case "error":
                    return Level.ERROR;
                case "audits":
                    return Level.DEBUG;
                case "info":
                    return Level.INFO;
                case "fatal":
                    return Level.FATAL;
                default:
                    return Level.ALL;
            }
        }

        private bool AuthenticateRequest(string authKey)
        {
            var key = Configuration.Settings.GetSetting(AuthKeyAppSettingName);

            if (!authKey.Equals(key))
            {
                Context.Response.StatusCode = (int) System.Net.HttpStatusCode.Forbidden;
                return false;
            }

            return true;
        }
    }
}
