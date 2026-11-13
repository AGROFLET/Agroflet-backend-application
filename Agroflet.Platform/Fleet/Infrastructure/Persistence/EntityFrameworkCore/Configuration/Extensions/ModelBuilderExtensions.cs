using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
namespace Agroflet.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyFleetConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Vehicle>().HasBaseType((Type?)null);
        builder.Entity<Vehicle>().HasKey(e => e.Id);
        builder.Entity<Vehicle>().Property(e => e.Version).IsConcurrencyToken();
        builder.Entity<Driver>().HasBaseType((Type?)null);
        builder.Entity<Driver>().HasKey(e => e.Id);
        builder.Entity<Driver>().Property(e => e.Version).IsConcurrencyToken();
        builder.Entity<Vehicle>().HasIndex(e => new { e.DispatcherId, e.Plate }).IsUnique();
        builder.Entity<Driver>().HasIndex(e => new { e.DispatcherId, e.Dni }).IsUnique();
        builder.Entity<Driver>().HasIndex(e => new { e.DispatcherId, e.LicenseNumber }).IsUnique();
        builder.Entity<Vehicle>().HasOne<User>().WithMany().HasForeignKey(e => e.DispatcherId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Driver>().HasOne<User>().WithMany().HasForeignKey(e => e.DispatcherId).OnDelete(DeleteBehavior.Restrict);
    }
}
