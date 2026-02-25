using ApexWebAPI.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace ApexWebAPI.Middleware
{
    public class HtmlSanitizeFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method is not ("POST" or "PUT"))
                return;

            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is null) continue;
                SanitizeObject(argument);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        private static void SanitizeObject(object obj)
        {
            if (obj is null) return;

            var type = obj.GetType();

            if (type.IsPrimitive || type == typeof(string) || type.IsEnum || type.IsValueType)
                return;

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead || !prop.CanWrite) continue;

                var value = prop.GetValue(obj);
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
