using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Agroflet.Platform.Tracking.Application.CommandServices;
using Agroflet.Platform.Tracking.Application.QueryServices;
using Agroflet.Platform.Tracking.Application.Internal.OutboundServices;
using Agroflet.Platform.Tracking.Domain.Model.Queries;
using Agroflet.Platform.Tracking.Interfaces.Rest.Resources;
using Agroflet.Platform.Tracking.Interfaces.Rest.Transform;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Tracking.Interfaces.Rest;

[ApiController, Authorize, Route("api/v1/positions"), Tags("Tracking")]
public class PositionsController(IReportedPositionQueryService queries, IReportedPositionCommandService commands, ICurrentUser user) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken ct) => this.FromResult(await queries.Handle(new CollectionQuery(Request.Query.ToDictionary(p => p.Key, p => p.Value.Select(v => v ?? "").ToArray())), ct), value => Ok((value).Select(ReportedPositionResourceFromEntityAssembler.ToResourceFromEntity)));
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id, CancellationToken ct) => this.FromResult(await queries.Handle(new GetReportedPositionByIdQuery(id), ct), value => Ok(ReportedPositionResourceFromEntityAssembler.ToResourceFromEntity(value)));
    [HttpPost]
    public async Task<ActionResult> Create(CreateReportedPositionResource resource, CancellationToken ct)
    {
        var result = await commands.Handle(CreateReportedPositionCommandFromResourceAssembler.ToCommandFromResource(resource, user.Id), ct);
        return this.FromResult(result, entity => CreatedAtAction(nameof(GetById), new { id = entity.Id }, ReportedPositionResourceFromEntityAssembler.ToResourceFromEntity(entity)));
    }

}
