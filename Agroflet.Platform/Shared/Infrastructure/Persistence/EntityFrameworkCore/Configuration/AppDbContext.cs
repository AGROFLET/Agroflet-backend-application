using Agroflet.Platform.Tracking.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Agroflet.Platform.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Agroflet.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Agroflet.Platform.Contact.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Agroflet.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Agroflet.Platform.Shipments.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
namespace Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors.AuditableEntityInterceptor());
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyTrackingConfiguration();
        builder.ApplyIncidentsConfiguration();
        builder.ApplyFleetConfiguration();
        builder.ApplyContactConfiguration();
        builder.ApplyIamConfiguration();
        builder.ApplyShipmentsConfiguration();
        foreach (var entity in builder.Model.GetEntityTypes())
            foreach (var property in entity.GetProperties())
            {
                if (property.ClrType == typeof(string)) property.SetMaxLength(property.Name is "Message" or "Description" or "CancellationReason" or "CargoDescription" or "Note" ? 2000 : property.Name is "PasswordHash" or "PayloadJson" ? 4000 : 255);
                if (property.ClrType == typeof(decimal)) { property.SetPrecision(12); property.SetScale(3); }
                if (property.ClrType == typeof(DateTime)) property.SetValueConverter(new ValueConverter<DateTime, DateTime>(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                if (property.ClrType == typeof(DateTime?)) property.SetValueConverter(new ValueConverter<DateTime?, DateTime?>(v => v.HasValue ? v.Value.ToUniversalTime() : v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v));
            }
        builder.UseSnakeCaseNamingConvention();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries().Where(e => e.State is EntityState.Added or EntityState.Modified))
            foreach (var property in entry.Properties)
                if (property.CurrentValue is DateTime timestamp)
                    property.CurrentValue = UtcTimestamp.Normalize(timestamp);

        return base.SaveChangesAsync(cancellationToken);
    }
}
