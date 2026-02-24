using ApexWebAPI.Concrete;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Infrastructure.ServiceExtensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApexDbContext>(opt =>
    {
        opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

    });
            return services;
        }
    }
}