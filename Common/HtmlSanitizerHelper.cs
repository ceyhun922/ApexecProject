using Ganss.XSS;

namespace ApexWebAPI.Common
{
    public static class HtmlSanitizerHelper
    {
        private static readonly HtmlSanitizer _sanitizer = new();

        public static string? Sanitize(string? input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return _sanitizer.Sanitize(input);
        }
    }
}
