using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
namespace Agroflet.Platform.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIncidentsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Incident>().HasBaseType((Type?)null);
        builder.Entity<Incident>().HasKey(e => e.Id);
        builder.Entity<Incident>().Property(e => e.Version).IsConcurrencyToken();
        builder.Entity<Notification>().HasBaseType((Type?)null);
        builder.Entity<Notification>().HasKey(e => e.Id);
        builder.Entity<Notification>().Property(e => e.Version).IsConcurrencyToken();
        builder.Entity<Incident>().HasIndex(e => new { e.ShipmentId, e.IdempotencyKey }).IsUnique();
        builder.Entity<Notification>().HasIndex(e => new { e.RecipientId, e.EventKey }).IsUnique();
        builder.Entity<Incident>().HasOne<Shipment>().WithMany().HasForeignKey(e => e.ShipmentId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Incident>().HasOne<User>().WithMany().HasForeignKey(e => e.ReportedBy).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Notification>().HasOne<Shipment>().WithMany().HasForeignKey(e => e.ShipmentId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Notification>().HasOne<User>().WithMany().HasForeignKey(e => e.RecipientId).OnDelete(DeleteBehavior.Restrict);
    }
}
