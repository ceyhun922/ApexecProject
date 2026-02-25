using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApexWebAPI.Infrastructure.Swagger
{
    public class CkEditorOperationFilter : IOperationFilter
    {
        private static readonly HashSet<string> _includedControllers = new(StringComparer.OrdinalIgnoreCase)
        {
            "SummerSchool", "LanguageCourse", "University"
        };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            
            // Yalnızca POST ve PUT metodları için açıklama ekleyelim
            if (method is not ("POST" or "PUT")) return;

            // Controller adını alalım
            var controller = context.ApiDescription.ActionDescriptor.RouteValues
                .TryGetValue("controller", out var c) ? c : string.Empty;

            // Eğer controller belirtilen listeye dahilse, açıklama ekleyelim
            if (_includedControllers.Contains(controller ?? string.Empty))
            {
                var existing = operation.Description ?? string.Empty;
                operation.Description = string.IsNullOrWhiteSpace(existing)
                    ? "✏️ **CKEditor destəklənir** — bu endpoint mətn sahələrində HTML qəbul edir. XSS qoruması avtomatik tətbiq olunur."
                    : existing + "\n\n✏️ **CKEditor destəklənir** — bu endpoint mətn sahələrində HTML qəbul edir. XSS qoruması avtomatik tətbiq olunur.";
            }
        }
    }
}