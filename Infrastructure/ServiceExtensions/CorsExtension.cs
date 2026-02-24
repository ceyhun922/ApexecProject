namespace ApexWebAPI.Infrastructure.ServiceExtensions
{
    public static class CorsExtension
    {
        public static IServiceCollection AddApexCors(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("ApexWebAPI", policy =>
                {
                    policy.WithOrigins(
                            "https://apexec.az",
                            "https://www.apexec.az",
                            "https://admin.apexec.az",
                            "http://localhost:3000",
                            "https://localhost:3000",
                            "http://localhost:3001",
                            "https://localhost:3001"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
