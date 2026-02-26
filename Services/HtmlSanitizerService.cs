using Ganss.XSS;

namespace ApexWebAPI.Infrastructure.Services
{
    public class HtmlSanitizerService
    {
        private readonly HtmlSanitizer _sanitizer;

        public HtmlSanitizerService()
        {
            _sanitizer = new HtmlSanitizer();

            // CKEditor tagları
            _sanitizer.AllowedTags.Add("figure");
            _sanitizer.AllowedTags.Add("figcaption");
            _sanitizer.AllowedTags.Add("iframe");
            _sanitizer.AllowedTags.Add("table");
            _sanitizer.AllowedTags.Add("thead");
            _sanitizer.AllowedTags.Add("tbody");
            _sanitizer.AllowedTags.Add("tfoot");
            _sanitizer.AllowedTags.Add("tr");
            _sanitizer.AllowedTags.Add("td");
            _sanitizer.AllowedTags.Add("th");
            _sanitizer.AllowedTags.Add("colgroup");
            _sanitizer.AllowedTags.Add("col");
            _sanitizer.AllowedTags.Add("caption");
            _sanitizer.AllowedTags.Add("blockquote");
            _sanitizer.AllowedTags.Add("pre");
            _sanitizer.AllowedTags.Add("code");
            _sanitizer.AllowedTags.Add("h1");
            _sanitizer.AllowedTags.Add("h2");
            _sanitizer.AllowedTags.Add("h3");
            _sanitizer.AllowedTags.Add("h4");
            _sanitizer.AllowedTags.Add("h5");
            _sanitizer.AllowedTags.Add("h6");
            _sanitizer.AllowedTags.Add("ul");
            _sanitizer.AllowedTags.Add("ol");
            _sanitizer.AllowedTags.Add("li");
            _sanitizer.AllowedTags.Add("strong");
            _sanitizer.AllowedTags.Add("em");
            _sanitizer.AllowedTags.Add("u");
            _sanitizer.AllowedTags.Add("s");
            _sanitizer.AllowedTags.Add("sub");
            _sanitizer.AllowedTags.Add("sup");
            _sanitizer.AllowedTags.Add("br");
            _sanitizer.AllowedTags.Add("hr");
            _sanitizer.AllowedTags.Add("p");
            _sanitizer.AllowedTags.Add("div");
            _sanitizer.AllowedTags.Add("span");
            _sanitizer.AllowedTags.Add("a");
            _sanitizer.AllowedTags.Add("img");

            // CKEditor atributları
            _sanitizer.AllowedAttributes.Add("class");
            _sanitizer.AllowedAttributes.Add("style");
            _sanitizer.AllowedAttributes.Add("src");
            _sanitizer.AllowedAttributes.Add("href");
            _sanitizer.AllowedAttributes.Add("alt");
            _sanitizer.AllowedAttributes.Add("title");
            _sanitizer.AllowedAttributes.Add("target");
            _sanitizer.AllowedAttributes.Add("width");
            _sanitizer.AllowedAttributes.Add("height");
            _sanitizer.AllowedAttributes.Add("allowfullscreen");
            _sanitizer.AllowedAttributes.Add("frameborder");
            _sanitizer.AllowedAttributes.Add("allow");
            _sanitizer.AllowedAttributes.Add("colspan");
            _sanitizer.AllowedAttributes.Add("rowspan");
            _sanitizer.AllowedAttributes.Add("data-image");

            // iframe üçün xüsusi icazə (YouTube, Vimeo)
            _sanitizer.AllowedSchemes.Add("https");
        }

        public string SanitizeHtmlContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return content;

            return _sanitizer.Sanitize(content);
        }
    }
}