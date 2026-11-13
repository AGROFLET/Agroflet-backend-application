using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Agroflet.Platform.Iam.Application.CommandServices;
using Agroflet.Platform.Iam.Application.QueryServices;
using Agroflet.Platform.Iam.Application.Internal.OutboundServices;
using Agroflet.Platform.Iam.Domain.Model.Queries;
using Agroflet.Platform.Iam.Interfaces.Rest.Resources;
using Agroflet.Platform.Iam.Interfaces.Rest.Transform;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Iam.Interfaces.Rest;

[ApiController, Authorize, Route("api/v1/users"), Tags("IAM")]
public class UsersController(IUserQueryService queries, ICurrentUser actor) : ControllerBase
{
    [HttpGet("profile")]
    public Task<ActionResult> OwnProfile(CancellationToken ct) => GetById(actor.Id, ct);
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken ct) => this.FromResult(await queries.Handle(new CollectionQuery(Request.Query.ToDictionary(p => p.Key, p => p.Value.Select(v => v ?? "").ToArray())), ct), value => Ok((value).Select(UserResourceFromEntityAssembler.ToResourceFromEntity)));
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id, CancellationToken ct) => this.FromResult(await queries.Handle(new GetUserByIdQuery(id), ct), value => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(value)));


}
