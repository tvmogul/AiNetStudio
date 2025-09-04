using System.Reflection;

namespace AiNetStudio.Helpers
{
    public static class VersionHelper
    {
        public static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
        }

        public static string GetInformationalVersion()
        {
            return Assembly
                .GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion ?? "Unknown";
        }

        public static string GetFormattedAppTitle(string baseTitle = "AiNetProfit")
        {
            string version = GetInformationalVersion();
            return $"{baseTitle} - v{version}";
        }
    }
}
