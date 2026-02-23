using System.Globalization;
using System.Security.Claims;
using System.Text;
using ApexWebAPI.Common;
using ApexWebAPI.Concrete;
using ApexWebAPI.Entities;
using ApexWebAPI.Middleware;
using ApexWebAPI.Repositories.Concrete;
using ApexWebAPI.Repositories.Interfaces;
using ApexWebAPI.Services.Concrete;
using ApexWebAPI.Services.Interfaces;
using ApexWebAPI.ValidationRule;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .WriteTo.Console()
        .WriteTo.File("logs/apex-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30)
        .Enrich.FromLogContext());

    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

    builder.Services.AddValidatorsFromAssemblyContaining<LoginValidation>();
    builder.Services.AddFluentValidationAutoValidation();

    builder.Services.AddControllers()
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

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApexWebAPI", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Bearer {token}",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
    });

    builder.Services.AddCors(opt =>
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

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                NameClaimType = ClaimTypes.NameIdentifier
            };
        });

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddDbContext<ApexDbContext>(opt =>
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor |
            ForwardedHeaders.XForwardedProto |
            ForwardedHeaders.XForwardedHost;

        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });

    // Repository & Unit of Work
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    // Application Services
    builder.Services.AddScoped<IHeroService, HeroService>();
    builder.Services.AddScoped<IAboutService, AboutService>();
    builder.Services.AddScoped<ITestimonialService, TestimonialService>();
    builder.Services.AddScoped<IFaqService, FaqService>();
    builder.Services.AddScoped<ICountryService, CountryService>();
    builder.Services.AddScoped<IDepartmentService, DepartmentService>();
    builder.Services.AddScoped<IEducationLevelService, EducationLevelService>();
    builder.Services.AddScoped<ISummerSchoolService, SummerSchoolService>();
    builder.Services.AddScoped<IContactService, ContactService>();
    builder.Services.AddScoped<IInformationService, InformationService>();
    builder.Services.AddScoped<IMessageService, MessageService>();
    builder.Services.AddScoped<IFooterService, FooterService>();
    builder.Services.AddScoped<IFileUploadService, FileUploadService>();
    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
    builder.Services.AddScoped<IEmailService, EmailService>();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApexDbContext>();

        if (!db.Users.Any())
        {
            db.Users.Add(new AppUser
            {
                Username = "admin",
                FullName = "Admin User",
                Email = "admin@apexec.az",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin"
            });
            db.SaveChanges();
        }
    }

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

    app.UseStaticFiles();

    app.UseRouting();

    app.UseCors("ApexWebAPI");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}
