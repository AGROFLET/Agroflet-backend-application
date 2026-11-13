using Agroflet.Platform.Contact.Domain.Model;
using Agroflet.Platform.Contact.Domain.Repositories;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
namespace Agroflet.Platform.Contact.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class ContactMessageRepository(AppDbContext context) : BaseRepository<ContactMessage>(context), IContactMessageRepository;
