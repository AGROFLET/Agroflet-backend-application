using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Agroflet.Platform.Incidents.Application.CommandServices;
using Agroflet.Platform.Incidents.Application.QueryServices;
using Agroflet.Platform.Incidents.Application.Internal.OutboundServices;
using Agroflet.Platform.Incidents.Domain.Model.Queries;
using Agroflet.Platform.Incidents.Interfaces.Rest.Resources;
using Agroflet.Platform.Incidents.Interfaces.Rest.Transform;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Incidents.Interfaces.Rest;

[ApiController, Authorize, Route("api/v1/incidents"), Tags("Incidents")]
public class IncidentsController(IIncidentQueryService queries, IIncidentCommandService commands, ICurrentUser user) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken ct) => this.FromResult(await queries.Handle(new CollectionQuery(Request.Query.ToDictionary(p => p.Key, p => p.Value.Select(v => v ?? "").ToArray())), ct), value => Ok((value).Select(IncidentResourceFromEntityAssembler.ToResourceFromEntity)));
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id, CancellationToken ct) => this.FromResult(await queries.Handle(new GetIncidentByIdQuery(id), ct), value => Ok(IncidentResourceFromEntityAssembler.ToResourceFromEntity(value)));
    [HttpPost]
    public async Task<ActionResult> Create(CreateIncidentResource resource, CancellationToken ct)
    {
        var result = await commands.Handle(CreateIncidentCommandFromResourceAssembler.ToCommandFromResource(resource, user.Id), ct);
        return this.FromResult(result, entity => CreatedAtAction(nameof(GetById), new { id = entity.Id }, IncidentResourceFromEntityAssembler.ToResourceFromEntity(entity)));
    }

}
