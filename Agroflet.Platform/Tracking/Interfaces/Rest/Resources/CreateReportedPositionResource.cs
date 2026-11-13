using System.ComponentModel.DataAnnotations;
namespace Agroflet.Platform.Tracking.Interfaces.Rest.Resources;

public record CreateReportedPositionResource(int ShipmentId, double Latitude, double Longitude, DateTime ReportedAt, [Required, StringLength(2000)] string Source, bool IsDemo, [Required(AllowEmptyStrings = true), StringLength(2000)] string Note, [StringLength(128)] string? EventId = null);
