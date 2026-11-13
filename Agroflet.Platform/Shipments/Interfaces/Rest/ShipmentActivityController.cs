using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
using Agroflet.Platform.Shipments.Application.CommandServices;
using Agroflet.Platform.Shipments.Application.QueryServices;
using Agroflet.Platform.Shipments.Domain.Model.Queries;
using Agroflet.Platform.Shipments.Interfaces.Rest.Transform;
using Agroflet.Platform.Incidents.Application.CommandServices;
using Agroflet.Platform.Incidents.Application.QueryServices;
using Agroflet.Platform.Incidents.Application.Internal.OutboundServices;
using Agroflet.Platform.Incidents.Interfaces.Rest.Resources;
using Agroflet.Platform.Incidents.Interfaces.Rest.Transform;
using Agroflet.Platform.Tracking.Application.CommandServices;
using Agroflet.Platform.Tracking.Application.QueryServices;
using Agroflet.Platform.Tracking.Application.Internal.OutboundServices;
using Agroflet.Platform.Tracking.Interfaces.Rest.Resources;
using Agroflet.Platform.Tracking.Interfaces.Rest.Transform;
namespace Agroflet.Platform.Shipments.Interfaces.Rest;

[ApiController, Authorize, Route("api/v1/shipments"), Tags("Shipment activity")]
public class ShipmentActivityController(IShipmentQueryService shipments, IStatusChangeQueryService history,
 IIncidentQueryService incidents, IIncidentCommandService incidentCommands, IReportedPositionQueryService positions,
 IReportedPositionCommandService positionCommands, ICurrentUser actor) : ControllerBase
{
    private static CollectionQuery Filter(int id, string sort = "id", string order = "asc") => new(new Dictionary<string, string[]> { { "shipmentId", [id.ToString()] }, { "_sort", [sort] }, { "_order", [order] } });
    [HttpGet("{id:int}/status-history")]
    public async Task<ActionResult> History(int id, CancellationToken ct) { (await shipments.Handle(new GetShipmentByIdQuery(id), ct)).RequireValue(); return Ok(((await history.Handle(Filter(id), ct)).RequireValue()).Select(StatusChangeResourceFromEntityAssembler.ToResourceFromEntity)); }
    [HttpGet("{id:int}/incidents")]
    public async Task<ActionResult> Incidents(int id, CancellationToken ct) { (await shipments.Handle(new GetShipmentByIdQuery(id), ct)).RequireValue(); return Ok(((await incidents.Handle(Filter(id, "occurredAt", "desc"), ct)).RequireValue()).Select(IncidentResourceFromEntityAssembler.ToResourceFromEntity)); }
    [HttpPost("{id:int}/incidents")]
    public async Task<ActionResult> Incident(int id, CreateIncidentResource resource, CancellationToken ct)
    {
        Guard.Require(resource.ShipmentId == 0 || resource.ShipmentId == id, "shipment-id-mismatch");
        var result = (await incidentCommands.Handle(CreateIncidentCommandFromResourceAssembler.ToCommandFromResource(resource with { ShipmentId = id }, actor.Id), ct)).RequireValue();
        var shipment = (await shipments.Handle(new GetShipmentByIdQuery(id), ct)).RequireValue();
        return StatusCode(201, new { incident = IncidentResourceFromEntityAssembler.ToResourceFromEntity(result), updatedEstimatedArrival = shipment.EstimatedArrivalAt });
    }
    [HttpGet("{id:int}/positions")]
    public async Task<ActionResult> Positions(int id, CancellationToken ct) { (await shipments.Handle(new GetShipmentByIdQuery(id), ct)).RequireValue(); return Ok(((await positions.Handle(Filter(id, "reportedAt"), ct)).RequireValue()).Select(ReportedPositionResourceFromEntityAssembler.ToResourceFromEntity)); }
    [HttpPost("{id:int}/positions")]
    public async Task<ActionResult> Position(int id, CreateReportedPositionResource resource, CancellationToken ct)
    {
        Guard.Require(resource.ShipmentId == 0 || resource.ShipmentId == id, "shipment-id-mismatch");
        Guard.Text(resource.EventId, "event-id", 128);
        var result = (await positionCommands.Handle(CreateReportedPositionCommandFromResourceAssembler.ToCommandFromResource(resource with { ShipmentId = id }, actor.Id), ct)).RequireValue();
        return StatusCode(201, ReportedPositionResourceFromEntityAssembler.ToResourceFromEntity(result));
    }
    [HttpGet("active/locations")]
    public async Task<ActionResult> ActiveLocations(CancellationToken ct)
    {
        var active = ((await shipments.Handle(new CollectionQuery(new Dictionary<string, string[]> { { "status", ["planned", "in_transit"] } }), ct)).RequireValue());
        var all = (await positions.Handle(new CollectionQuery(new Dictionary<string, string[]>()), ct)).RequireValue();
        return Ok(active.Select(s => { var p = all.Where(p => p.ShipmentId == s.Id).OrderByDescending(p => p.ReportedAt).ThenByDescending(p => p.Id).FirstOrDefault(); return new { shipmentId = s.Id, latitude = (double?)p?.Latitude, longitude = (double?)p?.Longitude, reportedAt = p?.ReportedAt, source = p?.Source, isStale = p == null || p.ReportedAt < DateTime.UtcNow.AddMinutes(-30), hasPosition = p != null }; }));
    }
}
