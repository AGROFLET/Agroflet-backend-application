using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Agroflet.Platform.Tests;

public class ApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection connection = new("Data Source=:memory:");
    private readonly Action<IServiceCollection>? configure;
    public ApiFactory(Action<IServiceCollection>? configure = null)
    {
        this.configure = configure;
        Environment.SetEnvironmentVariable("Jwt__Key", "test-only-signing-key-at-least-32-bytes-long");
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", "server=localhost;database=unused;user=unused;password=unused");
        Environment.SetEnvironmentVariable("PasswordReset__ReturnTokenInDevelopment", "true");
        connection.Open();
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureServices(services =>
        {
            configure?.Invoke(services);
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<AppDbContext>>();
            var mysql = Environment.GetEnvironmentVariable("TEST_MYSQL_CONNECTION");
            if (!string.IsNullOrEmpty(mysql))
            {
                var settings = new System.Data.Common.DbConnectionStringBuilder { ConnectionString = mysql };
                var database = settings.TryGetValue("database", out var value) ? Convert.ToString(value) : null;
                if (database is null || !System.Text.RegularExpressions.Regex.IsMatch(database, @"^agroflet_tests(?:_[A-Za-z0-9]+)*$"))
                    throw new InvalidOperationException("Tests may recreate only an explicitly named agroflet_tests[_suffix] database.");
            }
            services.AddDbContext<AppDbContext>((sp, o) => { o.AddInterceptors(sp.GetRequiredService<Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors.DispatchDomainEventsInterceptor>()); if (string.IsNullOrEmpty(mysql)) o.UseSqlite(connection); else o.UseMySQL(mysql); });
            using var provider = services.BuildServiceProvider(); using var scope = provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // TEST_MYSQL_CONNECTION must point to a disposable test database only.
            db.Database.EnsureDeleted();
            if (string.IsNullOrEmpty(mysql)) db.Database.EnsureCreated(); else db.Database.Migrate();
        });
    }
    protected override void Dispose(bool disposing) { base.Dispose(disposing); if (disposing) connection.Dispose(); }
}
