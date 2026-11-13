namespace Agroflet.Platform.Shipments.Domain.Model.Aggregates;

public partial class Shipment : IAuditableEntity
{
    DateTimeOffset? IAuditableEntity.CreatedAt
    {
        get => CreatedAt == default ? null : new DateTimeOffset(DateTime.SpecifyKind(CreatedAt, DateTimeKind.Utc));
        set => CreatedAt = value?.UtcDateTime ?? CreatedAt;
    }
    DateTimeOffset? IAuditableEntity.UpdatedAt
    {
        get => UpdatedAt == default ? null : new DateTimeOffset(DateTime.SpecifyKind(UpdatedAt, DateTimeKind.Utc));
        set => UpdatedAt = value?.UtcDateTime ?? UpdatedAt;
    }
}
