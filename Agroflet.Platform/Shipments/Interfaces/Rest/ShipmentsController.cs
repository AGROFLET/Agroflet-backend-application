using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Agroflet.Platform.Shipments.Application.CommandServices;
using Agroflet.Platform.Shipments.Application.QueryServices;
using Agroflet.Platform.Shipments.Domain.Model.Queries;
using Agroflet.Platform.Shipments.Interfaces.Rest.Resources;
using Agroflet.Platform.Shipments.Interfaces.Rest.Transform;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Shipments.Interfaces.Rest;

[ApiController, Authorize, Route("api/v1/shipments"), Tags("Shipments")]
public class ShipmentsController(IShipmentQueryService queries, IShipmentCommandService commands, ICurrentUser user, IShipmentPageService pages) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken ct)
    {
        var filters = Request.Query.ToDictionary(p => p.Key, p => p.Value.Select(v => v ?? "").ToArray());
        if (filters.Remove("legacy", out var legacy) && legacy.SequenceEqual(new[] { "true" }))
            return Ok(((await queries.Handle(new CollectionQuery(filters), ct)).RequireValue()).Select(ShipmentResourceFromEntityAssembler.ToResourceFromEntity));
        var result = await pages.Handle(new GetShipmentPageQuery(filters), ct);
        return this.FromResult(result, page => Ok(new { data = page.Data.Select(ShipmentResourceFromEntityAssembler.ToResourceFromEntity), page.TotalRecords, page.CurrentPage, page.PageSize, page.TotalPages }));
    }
    [HttpPatch("{id:int}/status")]
    public Task<ActionResult> Status(int id, UpdateShipmentResource resource, CancellationToken ct)
    {
        Guard.Require(resource.Status != null && resource.ExpectedVersion.HasValue, "status-and-version-required");
        return Update(id, resource, ct);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id, CancellationToken ct) => this.FromResult(await queries.Handle(new GetShipmentByIdQuery(id), ct), value => Ok(ShipmentResourceFromEntityAssembler.ToResourceFromEntity(value)));
    [HttpPost]
    public async Task<ActionResult> Create(CreateShipmentResource resource, CancellationToken ct)
    {
        var result = await commands.Handle(CreateShipmentCommandFromResourceAssembler.ToCommandFromResource(resource, user.Id), ct);
        return this.FromResult(result, entity => CreatedAtAction(nameof(GetById), new { id = entity.Id }, ShipmentResourceFromEntityAssembler.ToResourceFromEntity(entity)));
    }
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateShipmentResource resource, CancellationToken ct) => this.FromResult(await commands.Handle(UpdateShipmentCommandFromResourceAssembler.ToCommandFromResource(id, resource), ct), value => Ok(ShipmentResourceFromEntityAssembler.ToResourceFromEntity(value)));
}
