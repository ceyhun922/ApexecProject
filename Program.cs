using ApexWebAPI.Concrete;
using ApexWebAPI.Entities;
using ApexWebAPI.Infrastructure.MiddlewareExtensions;
using ApexWebAPI.Infrastructure.ServiceExtensions;
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
    builder.Services.AddControllersConfig();
    builder.Services.AddSwaggerWithBearer();
    builder.Services.AddApexCors();
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddDatabase(builder.Configuration);
    builder.Services.AddForwardedHeadersConfig();
    builder.Services.AddApplicationServices(builder.Configuration);

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

    app.UseApexMiddleware();

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
