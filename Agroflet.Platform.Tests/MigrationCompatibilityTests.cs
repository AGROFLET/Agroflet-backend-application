using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Xunit;

namespace Agroflet.Platform.Tests;

public class MigrationCompatibilityTests
{
    [Fact]
    public async Task PreviousInitialMigration_UpgradesWithoutLosingExistingRows()
    {
        var connection = Environment.GetEnvironmentVariable("TEST_MYSQL_CONNECTION");
        if (string.IsNullOrWhiteSpace(connection)) return; // MySQL-specific upgrade verification.
        var settings = new System.Data.Common.DbConnectionStringBuilder { ConnectionString = connection };
        var database = settings.TryGetValue("database", out var value) ? Convert.ToString(value) : null;
        Assert.Matches(@"^agroflet_tests(?:_[A-Za-z0-9]+)*$", database ?? "");
        settings["database"] = database + "_upgrade";
        var options = new DbContextOptionsBuilder<AppDbContext>().UseMySQL(settings.ConnectionString).Options;
        await using var db = new AppDbContext(options);
        await db.Database.EnsureDeletedAsync();
        var migrator = db.GetService<IMigrator>();
        await migrator.MigrateAsync("20261019220925_InitialCreate");
        await db.Database.ExecuteSqlRawAsync("INSERT INTO locations (id,name,city,region,latitude,longitude,kind,version) VALUES (9999,'Preserved location','Lima','Lima',-12,-77,'origin',1)");
        await migrator.MigrateAsync();
        Assert.False(db.Database.HasPendingModelChanges());
        Assert.Empty(await db.Database.GetPendingMigrationsAsync());
        var names = await db.Database.SqlQueryRaw<string>("SELECT name AS Value FROM locations WHERE id=9999").ToListAsync();
        Assert.Equal("Preserved location", Assert.Single(names));
        var tables = await db.Database.SqlQueryRaw<string>("SELECT TABLE_NAME AS Value FROM information_schema.tables WHERE TABLE_SCHEMA=DATABASE()").ToListAsync();
        Assert.Contains("contact_messages", tables);
        Assert.Contains("revoked_sessions", tables);
        var columns = await db.Database.SqlQueryRaw<string>("SELECT COLUMN_NAME AS Value FROM information_schema.columns WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='reported_positions'").ToListAsync();
        Assert.Contains("event_id", columns);
        Assert.Contains("20261019220925_InitialCreate", await db.Database.GetAppliedMigrationsAsync());
        // A second upgrade must be harmless too.
        await migrator.MigrateAsync();
        await db.Database.EnsureDeletedAsync();
    }
}
