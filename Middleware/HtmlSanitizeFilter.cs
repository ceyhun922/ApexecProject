using ApexWebAPI.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace ApexWebAPI.Middleware
{
    public class HtmlSanitizeFilter : IActionFilter
    {
        private static readonly HashSet<string> _excludedControllers = new(StringComparer.OrdinalIgnoreCase)
        {
            "Auth"
        };

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method is not ("POST" or "PUT"))
                return;

            var controller = context.RouteData.Values["controller"]?.ToString();
            if (_excludedControllers.Contains(controller ?? string.Empty))
                return;

            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is null) continue;
                SanitizeObject(argument);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        private static readonly HashSet<Type> _skipTypes = new()
        {
            typeof(IFormFile), typeof(Stream), typeof(CancellationToken)
        };

        private static void SanitizeObject(object obj)
        {
            if (obj is null) return;

            var type = obj.GetType();

            if (type.IsPrimitive || type == typeof(string) || type.IsEnum || type.IsValueType)
                return;

            if (_skipTypes.Any(t => t.IsAssignableFrom(type)))
                return;

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.GetIndexParameters().Length > 0) continue;
                if (!prop.CanRead || !prop.CanWrite) continue;
                if (_skipTypes.Any(t => t.IsAssignableFrom(prop.PropertyType))) continue;

                // [SkipSanitize] ile işaretli proplar CKEditor alanlarıdır;
                // controller'da SanitizeDescription() ile ayrıca işlenirler.
                if (prop.IsDefined(typeof(SkipSanitizeAttribute), inherit: false)) continue;

                object? value;
                try { value = prop.GetValue(obj); }
                catch { continue; }

                if (value is null) continue;

                if (prop.PropertyType == typeof(string))
                {
                    var sanitized = HtmlSanitizerHelper.Sanitize(value as string);
                    prop.SetValue(obj, sanitized);
                }
                else if (!prop.PropertyType.IsPrimitive && !prop.PropertyType.IsEnum && !prop.PropertyType.IsValueType)
                {
                    SanitizeObject(value);
                }
            }
        }
    }
}
