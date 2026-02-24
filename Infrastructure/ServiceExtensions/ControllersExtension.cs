using ApexWebAPI.ValidationRule;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ApexWebAPI.Infrastructure.ServiceExtensions
{
    public static class ControllersExtension
    {
        public static IServiceCollection AddControllersConfig(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<LoginValidation>();
            services.AddFluentValidationAutoValidation();

            services.AddControllers(options =>
                {
                    options.Filters.Add(new AuthorizeFilter());
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(e => e.Value!.Errors.Count > 0)
                            .ToDictionary(
                                e => e.Key,
                                e => e.Value!.Errors.Select(x => x.ErrorMessage).ToArray()
                            );

                        return new BadRequestObjectResult(errors);
                    };
                });

            return services;
        }
    }
}
