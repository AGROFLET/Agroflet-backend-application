using Agroflet.Platform.Contact.Domain.Model.Commands;
using Agroflet.Platform.Contact.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Contact.Interfaces.Rest.Transform;

public static class CreateContactMessageCommandFromResourceAssembler
{
    public static CreateContactMessageCommand ToCommandFromResource(CreateContactMessageResource resource) => new(resource.Name, resource.Email, resource.Message);
}
