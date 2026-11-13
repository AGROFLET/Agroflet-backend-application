using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
namespace Agroflet.Platform.Tracking.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyTrackingConfiguration(this ModelBuilder builder)
    {
        builder.Entity<ReportedPosition>().HasIndex(e => new { e.ShipmentId, e.EventId }).IsUnique();
        builder.Entity<ReportedPosition>().HasBaseType((Type?)null);
        builder.Entity<ReportedPosition>().HasKey(e => e.Id);
        builder.Entity<ReportedPosition>().Property(e => e.Version).IsConcurrencyToken();
        builder.Entity<ReportedPosition>().HasOne<Shipment>().WithMany().HasForeignKey(e => e.ShipmentId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<ReportedPosition>().HasOne<User>().WithMany().HasForeignKey(e => e.RecordedBy).OnDelete(DeleteBehavior.Restrict);
    }
}
