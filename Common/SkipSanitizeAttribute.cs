namespace ApexWebAPI.Common
{
    /// <summary>
    /// Marks a DTO property so that HtmlSanitizeFilter skips it.
    /// Use on CKEditor (rich HTML) fields that are sanitized separately
    /// by HtmlSanitizerService.SanitizeDescription().
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SkipSanitizeAttribute : Attribute { }
}
