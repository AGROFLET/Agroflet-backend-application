using System.ComponentModel.DataAnnotations;
namespace Agroflet.Platform.Incidents.Interfaces.Rest.Resources;

public record CreateIncidentResource(int ShipmentId, [Required, StringLength(2000)] string Type, [Required, StringLength(2000)] string Description, int EstimatedDelayMinutes, DateTime OccurredAt, [Required, StringLength(2000)] string IdempotencyKey);
