using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApexWebAPI.Infrastructure.Swagger
{
    public class CkEditorOperationFilter : IOperationFilter
    {
        private static readonly HashSet<string> _excluded = new(StringComparer.OrdinalIgnoreCase)
        {
            "FileImages", "Auth"
        };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            if (method is not ("POST" or "PUT")) return;

            var controller = context.ApiDescription.ActionDescriptor.RouteValues
                .TryGetValue("controller", out var c) ? c : string.Empty;

            if (_excluded.Contains(controller ?? string.Empty)) return;

            // Endpoint açıklamasını ekleyelim
            var existing = operation.Description ?? string.Empty;
            operation.Description = string.IsNullOrWhiteSpace(existing)
                ? "✏️ **CKEditor destəklənir** — bu endpoint mətn sahələrində HTML qəbul edir. XSS qoruması avtomatik tətbiq olunur."
                : existing + "\n\n✏️ **CKEditor destəklənir** — bu endpoint mətn sahələrində HTML qəbul edir. XSS qoruması avtomatik tətbiq olunur.";
        }
    }
}