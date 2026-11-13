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

[ApiController, Authorize, Route("api/v1/notifications"), Tags("Incidents")]
public class NotificationsController(INotificationQueryService queries, INotificationCommandService commands) : ControllerBase
{
    [HttpPatch("{id:int}/read")]
    public Task<ActionResult> Read(int id, CancellationToken ct) => Update(id, new UpdateNotificationResource(true), ct);
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken ct) => this.FromResult(await queries.Handle(new CollectionQuery(Request.Query.ToDictionary(p => p.Key, p => p.Value.Select(v => v ?? "").ToArray())), ct), value => Ok((value).Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity)));
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id, CancellationToken ct) => this.FromResult(await queries.Handle(new GetNotificationByIdQuery(id), ct), value => Ok(NotificationResourceFromEntityAssembler.ToResourceFromEntity(value)));

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateNotificationResource resource, CancellationToken ct) => this.FromResult(await commands.Handle(UpdateNotificationCommandFromResourceAssembler.ToCommandFromResource(id, resource), ct), value => Ok(NotificationResourceFromEntityAssembler.ToResourceFromEntity(value)));
}
