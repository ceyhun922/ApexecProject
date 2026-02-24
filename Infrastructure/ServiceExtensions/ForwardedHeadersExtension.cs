using Microsoft.AspNetCore.HttpOverrides;

namespace ApexWebAPI.Infrastructure.ServiceExtensions
{
    public static class ForwardedHeadersExtension
    {
        public static IServiceCollection AddForwardedHeadersConfig(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor |
            ForwardedHeaders.XForwardedProto |
            ForwardedHeaders.XForwardedHost;

        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });
    return services;
        }
    }
}