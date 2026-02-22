using System.Globalization;
using System.Security.Claims;
using System.Text;
using ApexWebAPI.Concrete;
using ApexWebAPI.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
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
            new OpenApiSecurityScheme { Reference = new OpenApiReference 
                { Type = ReferenceType.SecurityScheme, Id = "Bearer" }},
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
                "https://localhost:3000"     
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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<ApexDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApexDbContext>();
    db.Database.Migrate();
     if (!db.Users.Any())
    {
        db.Users.Add(new AppUser
        {Username = "admin",
        FullName = "Admin User",
        Email = "admin@apexec.az",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
        Role = "Admin"
        });
        db.SaveChanges();
    }
}

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
    var langSegment = segments.Length > 3 ? segments[3] : "az";
    var culture = supportedCultures.Contains(langSegment) ? langSegment : "az";

    var cultureInfo = new CultureInfo(culture);
    CultureInfo.CurrentCulture = cultureInfo;
    CultureInfo.CurrentUICulture = cultureInfo;

    await next.Invoke();
});




app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseCors("ApexWebAPI");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();