using ApexWebAPI.Common;
using ApexWebAPI.Repositories.Concrete;
using ApexWebAPI.Repositories.Interfaces;
using ApexWebAPI.Services.Concrete;
using ApexWebAPI.Services.Interfaces;

namespace ApexWebAPI.Infrastructure.ServiceExtensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IHeroService, HeroService>();
            services.AddScoped<IAboutService, AboutService>();
            services.AddScoped<ITestimonialService, TestimonialService>();
            services.AddScoped<IFaqService, FaqService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEducationLevelService, EducationLevelService>();
            services.AddScoped<ISummerSchoolService, SummerSchoolService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IInformationService, InformationService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IFileUploadService, FileUploadService>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
