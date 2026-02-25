using Ganss.XSS;

namespace ApexWebAPI.Infrastructure.Services
{
    public class HtmlSanitizerService
    {
        private readonly HtmlSanitizer _sanitizer;

        public HtmlSanitizerService()
        {
            _sanitizer = new HtmlSanitizer();
        }

        // HTML içeriğini güvenli hale getiren metod
        public string SanitizeHtmlContent(string content)
        {
            return _sanitizer.Sanitize(content);  // HTML içeriğini temizler
        }
    }
}