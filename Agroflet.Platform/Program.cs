using Agroflet.Platform.Iam.Application.CommandServices;
using Agroflet.Platform.Iam.Application.QueryServices;
using Agroflet.Platform.Iam.Application.Internal.OutboundServices;
using Agroflet.Platform.Iam.Application.Internal.QueryServices;
using Agroflet.Platform.Iam.Domain.Repositories;
using Agroflet.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Agroflet.Platform.Fleet.Application.CommandServices;
using Agroflet.Platform.Fleet.Application.QueryServices;
using Agroflet.Platform.Fleet.Application.Internal.QueryServices;
using Agroflet.Platform.Fleet.Domain.Repositories;
using Agroflet.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Agroflet.Platform.Shipments.Application.CommandServices;
using Agroflet.Platform.Shipments.Application.QueryServices;
using Agroflet.Platform.Shipments.Application.Internal.QueryServices;
using Agroflet.Platform.Shipments.Domain.Repositories;
using Agroflet.Platform.Shipments.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Agroflet.Platform.Tracking.Application.CommandServices;
using Agroflet.Platform.Tracking.Application.QueryServices;
using Agroflet.Platform.Tracking.Application.Internal.OutboundServices;
using Agroflet.Platform.Tracking.Application.Internal.QueryServices;
using Agroflet.Platform.Tracking.Domain.Repositories;
using Agroflet.Platform.Tracking.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Agroflet.Platform.Incidents.Application.CommandServices;
using Agroflet.Platform.Incidents.Application.QueryServices;
using Agroflet.Platform.Incidents.Application.Internal.OutboundServices;
using Agroflet.Platform.Incidents.Application.Internal.QueryServices;
using Agroflet.Platform.Incidents.Domain.Repositories;
using Agroflet.Platform.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Agroflet.Platform.Iam.Application.Internal.CommandServices;
using Agroflet.Platform.Fleet.Application.Internal.CommandServices;
using Agroflet.Platform.Shipments.Application.Internal.CommandServices;
using Agroflet.Platform.Tracking.Application.Internal.CommandServices;
using Agroflet.Platform.Incidents.Application.Internal.CommandServices;
using Cortex.Mediator.DependencyInjection;
using System.Text;
using System.Security.Claims;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Agroflet.Platform.Iam.Infrastructure.Authentication;
using Agroflet.Platform.Iam.Infrastructure.Email;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Repositories;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
    builder.Configuration.AddEnvironmentVariables();
    builder.Configuration.AddCommandLine(args);
}
builder.Services.AddCortexMediator([typeof(Program)]);
builder.Services.AddScoped<Agroflet.Platform.Incidents.Interfaces.Acl.IIncidentsContextFacade, Agroflet.Platform.Incidents.Application.Acl.IncidentsContextFacade>();
builder.Services.AddScoped<Agroflet.Platform.Fleet.Interfaces.Acl.IFleetContextFacade, Agroflet.Platform.Fleet.Application.Acl.FleetContextFacade>();
builder.Services.AddScoped<Agroflet.Platform.Iam.Interfaces.Acl.IIamContextFacade, Agroflet.Platform.Iam.Application.Acl.IamContextFacade>();
builder.Services.AddScoped<Agroflet.Platform.Shipments.Interfaces.Acl.IShipmentsContextFacade, Agroflet.Platform.Shipments.Application.Acl.ShipmentsContextFacade>();
var jwtKey = builder.Configuration["Jwt:Key"] ?? "";
if (Encoding.UTF8.GetByteCount(jwtKey) < 32) throw new InvalidOperationException("Set Jwt__Key to a random secret of at least 32 bytes.");
builder.Services.AddRouting(o => o.LowercaseUrls = true);
builder.Services.AddControllers(o => o.Conventions.Add(new KebabCaseRouteNamingConvention()));
builder.Services.AddOpenApi(options => options.AddDocumentTransformer((document, context, ct) =>
{
    OpenApiConfiguration.Configure(document);
    return Task.CompletedTask;
}));
builder.Services.AddLocalization(o => o.ResourcesPath = "Resources");
builder.Services.AddProblemDetails(); builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors.DispatchDomainEventsInterceptor>();
builder.Services.AddDbContext<AppDbContext>((sp, o) => o.AddInterceptors(sp.GetRequiredService<Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors.DispatchDomainEventsInterceptor>()).UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Set ConnectionStrings__DefaultConnection.")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>(); builder.Services.AddScoped<IAccessScope, AccessScope>();
builder.Services.AddScoped<IHashingService, Agroflet.Platform.Iam.Infrastructure.Hashing.BCrypt.Services.HashingService>();
builder.Services.AddScoped<ITokenService, Agroflet.Platform.Iam.Infrastructure.Tokens.Jwt.Services.TokenService>();
builder.Services.AddScoped<ICredentialService, CredentialService>(); builder.Services.AddScoped<IPasswordResetDelivery, PasswordResetDelivery>();
builder.Services.AddScoped<IPasswordResetRequestRepository, PasswordResetRequestRepository>(); builder.Services.AddScoped<IAuthenticationCommandService, AuthenticationCommandService>();
builder.Services.AddScoped<INotificationPublisher, NotificationPublisher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleQueryService, VehicleQueryService>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IDriverQueryService, DriverQueryService>();
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddScoped<IShipmentQueryService, ShipmentQueryService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationQueryService, LocationQueryService>();
builder.Services.AddScoped<IStatusChangeRepository, StatusChangeRepository>();
builder.Services.AddScoped<IStatusChangeQueryService, StatusChangeQueryService>();
builder.Services.AddScoped<IReportedPositionRepository, ReportedPositionRepository>();
builder.Services.AddScoped<IReportedPositionQueryService, ReportedPositionQueryService>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<IIncidentQueryService, IncidentQueryService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationQueryService, NotificationQueryService>();
builder.Services.AddScoped<IVehicleCommandService, VehicleCommandService>();
builder.Services.AddScoped<IDriverCommandService, DriverCommandService>();
builder.Services.AddScoped<IShipmentCommandService, ShipmentCommandService>();
builder.Services.AddScoped<IReportedPositionCommandService, ReportedPositionCommandService>();
builder.Services.AddScoped<IIncidentCommandService, IncidentCommandService>();
builder.Services.AddScoped<INotificationCommandService, NotificationCommandService>();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? []).AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true, ValidateIssuerSigningKey = true, ValidIssuer = builder.Configuration["Jwt:Issuer"], ValidAudience = builder.Configuration["Jwt:Audience"], IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), ClockSkew = TimeSpan.FromSeconds(30) };
    o.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var id = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            var users = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var user = int.TryParse(id, out var key) ? await users.FindByIdAsync(key, context.HttpContext.RequestAborted) : null;
            var revocations = context.HttpContext.RequestServices.GetRequiredService<ISessionRevocationService>();
            var jti = context.Principal?.FindFirstValue("jti");
            if (jti == null || await revocations.IsRevoked(jti, context.HttpContext.RequestAborted)) { context.Fail("Session revoked."); return; }
            if (user == null || user.TokenVersion.ToString() != context.Principal?.FindFirstValue("version")) context.Fail("Session revoked.");
        }
    };
});
builder.Services.AddScoped<ISessionRevocationService, SessionRevocationService>();
builder.Services.AddScoped<Agroflet.Platform.Contact.Application.CommandServices.IContactCommandService, Agroflet.Platform.Contact.Application.Internal.CommandServices.ContactCommandService>();
builder.Services.AddScoped<Agroflet.Platform.Contact.Domain.Repositories.IContactMessageRepository, Agroflet.Platform.Contact.Infrastructure.Persistence.EntityFrameworkCore.Repositories.ContactMessageRepository>();
builder.Services.AddScoped<IShipmentPageService, Agroflet.Platform.Shipments.Application.Internal.QueryServices.ShipmentPageService>();
builder.Services.AddHttpClient<IRouteProvider, Agroflet.Platform.Tracking.Infrastructure.Routing.MapboxRouteProvider>().RemoveAllLoggers();
builder.Services.AddAuthorization();
builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = 429;
    o.AddPolicy("contact", context => RateLimitPartition.GetFixedWindowLimiter(context.Connection.RemoteIpAddress?.ToString() ?? "unknown", _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
    o.AddPolicy("auth", context => RateLimitPartition.GetFixedWindowLimiter(context.Connection.RemoteIpAddress?.ToString() ?? "unknown", _ => new FixedWindowRateLimiterOptions { PermitLimit = 20, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
});
builder.Services.AddScoped<Agroflet.Platform.Shipments.Domain.Repositories.IShipmentPageRepository, Agroflet.Platform.Shipments.Infrastructure.Persistence.EntityFrameworkCore.Repositories.ShipmentPageRepository>();
builder.Services.AddScoped<Agroflet.Platform.Tracking.Application.QueryServices.IPlannedRouteQueryService, Agroflet.Platform.Tracking.Application.Internal.QueryServices.PlannedRouteQueryService>();
var app = builder.Build();
if (args.Contains("--migrate"))
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync(); return;
}
if (builder.Configuration.GetValue<bool>("Database:ApplyMigrations"))
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
}
app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture("es").AddSupportedCultures("es", "en").AddSupportedUICultures("es", "en"));
app.UseExceptionHandler(); app.UseCors(); app.UseRateLimiter(); app.UseAuthentication(); app.UseAuthorization();
app.MapGet("/health/live", () => Results.Ok(new { status = "ok" }));
app.MapGet("/health/ready", async (AppDbContext db, CancellationToken ct) => await db.Database.CanConnectAsync(ct) ? Results.Ok(new { status = "ready" }) : Results.StatusCode(503));
app.MapOpenApi(); app.MapScalarApiReference(); app.MapControllers(); app.Run();
public partial class Program { }


