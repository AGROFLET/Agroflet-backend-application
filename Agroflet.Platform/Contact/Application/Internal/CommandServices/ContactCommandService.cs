using Agroflet.Platform.Contact.Application.CommandServices;
using Agroflet.Platform.Contact.Domain.Model;
using Agroflet.Platform.Contact.Domain.Model.Commands;
using Agroflet.Platform.Contact.Domain.Repositories;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Contact.Application.Internal.CommandServices;

public sealed class ContactCommandService(IContactMessageRepository repository, IUnitOfWork unitOfWork, ILogger<ContactCommandService> logger) : IContactCommandService
{
    public Task<Result<ContactMessage>> Handle(CreateContactMessageCommand command, CancellationToken ct) => Result<ContactMessage>.ExecuteAsync(async () =>
    {
        var entity = new ContactMessage(command.Name, command.Email, command.Message);
        try { await repository.AddAsync(entity, ct); await unitOfWork.CompleteAsync(ct); }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogWarning("Contact persistence failed: {ErrorType}", ex.GetType().Name);
            throw new BusinessException("contact-unavailable", 503);
        }
        return entity;
    });
}
