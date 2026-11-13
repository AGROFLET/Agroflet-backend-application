using Agroflet.Platform.Contact.Application.CommandServices;
using Agroflet.Platform.Contact.Interfaces.Rest.Resources;
using Agroflet.Platform.Contact.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
namespace Agroflet.Platform.Contact.Interfaces.Rest;

[ApiController, AllowAnonymous, Route("api/v1/contact-messages"), Tags("Contact")]
public sealed class ContactMessagesController(IContactCommandService service) : ControllerBase
{
    [HttpPost, EnableRateLimiting("contact")]
    public async Task<ActionResult> Create(CreateContactMessageResource resource, CancellationToken ct)
        => this.FromResult(await service.Handle(CreateContactMessageCommandFromResourceAssembler.ToCommandFromResource(resource), ct),
            entity => StatusCode(201, new ContactReceiptResource(entity.Id, entity.ReceivedAt)));
}
