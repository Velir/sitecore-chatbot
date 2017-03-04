namespace Sitecore.ChatBot.Constants
{
    public static class AppConstants
    {
        public const string AppInsightsIdKey = "AppInsightsAppId";
        public const string AppInsightsApiKey = "AppInsightsApiKey";

        public const string AppInsightsEndpoint = "https://api.applicationinsights.io/beta/apps/{0}/{1}/{2}";

        public const string CCServerEndpointKey = "Sitecore.CommandControl.Endpoint";
        public const string CCServerAuthKey = "Sitecore.CommandControl.AuthKey";
        public const string AppInsightsQueryEndpoint = "https://api.applicationinsights.io/beta/apps/{0}/query?query={1}";
    }
}