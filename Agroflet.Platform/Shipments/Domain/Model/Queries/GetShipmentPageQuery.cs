using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
namespace Agroflet.Platform.Shipments.Domain.Model.Queries;

public record GetShipmentPageQuery(IReadOnlyDictionary<string, string[]> Filters);
public record ShipmentPage(IReadOnlyList<Shipment> Data, int TotalRecords, int CurrentPage, int PageSize, int TotalPages);
