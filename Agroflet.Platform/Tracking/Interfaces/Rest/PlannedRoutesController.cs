using Agroflet.Platform.Tracking.Application.QueryServices;
using Agroflet.Platform.Tracking.Domain.Model.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Agroflet.Platform.Tracking.Interfaces.Rest;

[ApiController, Authorize, Route("api/v1/shipments"), Tags("Tracking")]
public sealed class PlannedRoutesController(IPlannedRouteQueryService service) : ControllerBase
{
    [HttpGet("{id:int}/planned-route")]
    public async Task<ActionResult> Get(int id, CancellationToken ct)
        => this.FromResult(await service.Handle(new GetPlannedRouteQuery(id), ct), route => Ok(route));
}
