using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Agroflet.Platform.Fleet.Application.CommandServices;
using Agroflet.Platform.Fleet.Application.QueryServices;
using Agroflet.Platform.Fleet.Domain.Model.Queries;
using Agroflet.Platform.Fleet.Interfaces.Rest.Resources;
using Agroflet.Platform.Fleet.Interfaces.Rest.Transform;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Fleet.Interfaces.Rest;

[ApiController, Authorize, Route("api/v1/drivers"), Tags("Fleet")]
public class DriversController(IDriverQueryService queries, IDriverCommandService commands, ICurrentUser user) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken ct) => this.FromResult(await queries.Handle(new CollectionQuery(Request.Query.ToDictionary(p => p.Key, p => p.Value.Select(v => v ?? "").ToArray())), ct), value => Ok((value).Select(e => DriverResourceFromEntityAssembler.ToResourceFromEntity(e, user.Role == "dispatcher"))));
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id, CancellationToken ct) => this.FromResult(await queries.Handle(new GetDriverByIdQuery(id), ct), value => Ok(DriverResourceFromEntityAssembler.ToResourceFromEntity(value, user.Role == "dispatcher")));
    [HttpPost]
    public async Task<ActionResult> Create(CreateDriverResource resource, CancellationToken ct)
    {
        var result = await commands.Handle(CreateDriverCommandFromResourceAssembler.ToCommandFromResource(resource, user.Id), ct);
        return this.FromResult(result, entity => CreatedAtAction(nameof(GetById), new { id = entity.Id }, DriverResourceFromEntityAssembler.ToResourceFromEntity(entity)));
    }
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateDriverResource resource, CancellationToken ct) => this.FromResult(await commands.Handle(UpdateDriverCommandFromResourceAssembler.ToCommandFromResource(id, resource), ct), value => Ok(DriverResourceFromEntityAssembler.ToResourceFromEntity(value)));
}
