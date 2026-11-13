using Agroflet.Platform.Contact.Domain.Model;
using Agroflet.Platform.Contact.Domain.Model.Commands;
namespace Agroflet.Platform.Contact.Application.CommandServices;

public interface IContactCommandService { Task<Result<ContactMessage>> Handle(CreateContactMessageCommand command, CancellationToken ct); }
