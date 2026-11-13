using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
namespace Agroflet.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Agroflet.Platform.Iam.Domain.Model.Aggregates.RevokedSession>().HasKey(e => e.Jti);
        builder.Entity<Agroflet.Platform.Iam.Domain.Model.Aggregates.RevokedSession>().HasIndex(e => e.ExpiresAt);
        builder.Entity<User>().HasBaseType((Type?)null);
        builder.Entity<User>().HasKey(e => e.Id);
        builder.Entity<User>().Property(e => e.Version).IsConcurrencyToken();
        builder.Entity<PasswordResetRequest>().HasBaseType((Type?)null);
        builder.Entity<PasswordResetRequest>().HasKey(e => e.Id);
        builder.Entity<PasswordResetRequest>().Property(e => e.Version).IsConcurrencyToken();
        builder.Entity<User>().HasIndex(e => e.Email).IsUnique();
        builder.Entity<PasswordResetRequest>().HasIndex(e => e.TokenHash).IsUnique();
        builder.Entity<PasswordResetRequest>().HasOne<User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
