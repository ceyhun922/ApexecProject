using System.Globalization;
using ApexWebAPI.Middleware;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using Serilog;

namespace ApexWebAPI.Infrastructure.MiddlewareExtensions
{
    public static class AppMiddlewareExtension
    {
        public static WebApplication UseApexMiddleware(this WebApplication app)
        {
            app.UseExceptionHandling();

            app.UseForwardedHeaders();

            var supportedCultures = new[] { "en", "tr", "ru", "az" };

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("az"),
                SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
                SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
            };

            app.UseRequestLocalization(localizationOptions);

            app.Use(async (context, next) =>
            {
                var segments = context.Request.Path.Value?.Split('/') ?? Array.Empty<string>();
                var langSegment = segments.Length > 2 ? segments[2] : "az";
                var culture = supportedCultures.Contains(langSegment) ? langSegment : "az";

                var cultureInfo = new CultureInfo(culture);
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;

                await next.Invoke();
            });

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI();

            // Static files: configured upload path or fallback to wwwroot
            var config = app.Configuration;
            var uploadPath = config["App:UploadPath"];
            var staticRoot = !string.IsNullOrWhiteSpace(uploadPath)
                ? uploadPath
                : Path.Combine(app.Environment.ContentRootPath, "wwwroot");

            Directory.CreateDirectory(Path.Combine(staticRoot, "images"));
            Directory.CreateDirectory(Path.Combine(staticRoot, "videos"));

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticRoot),
                RequestPath = ""
            });

            app.UseRouting();

            app.UseCors("ApexWebAPI");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
