namespace ApexWebAPI.Common
{
    public static class LanguageCodes
    {
        public const string Az = "az";
        public const string En = "en";
        public const string Ru = "ru";
        public const string Tr = "tr";

        public static readonly string[] All = { Az, En, Ru, Tr };

        public static bool IsSupported(string lang) =>
            All.Contains(lang, StringComparer.OrdinalIgnoreCase);

        public static string Fallback(string lang) =>
            IsSupported(lang) ? lang : Az;
    }
}
