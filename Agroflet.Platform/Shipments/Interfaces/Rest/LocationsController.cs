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

[ApiController, Authorize, Route("api/v1/locations"), Tags("Shipments")]
public class LocationsController(ILocationQueryService queries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken ct) => this.FromResult(await queries.Handle(new CollectionQuery(Request.Query.ToDictionary(p => p.Key, p => p.Value.Select(v => v ?? "").ToArray())), ct), value => Ok((value).Select(LocationResourceFromEntityAssembler.ToResourceFromEntity)));
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id, CancellationToken ct) => this.FromResult(await queries.Handle(new GetLocationByIdQuery(id), ct), value => Ok(LocationResourceFromEntityAssembler.ToResourceFromEntity(value)));


}
