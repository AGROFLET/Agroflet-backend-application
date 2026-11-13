using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
namespace Agroflet.Platform.Contact.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyContactConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Agroflet.Platform.Contact.Domain.Model.ContactMessage>().HasKey(e => e.Id);
    }
}
