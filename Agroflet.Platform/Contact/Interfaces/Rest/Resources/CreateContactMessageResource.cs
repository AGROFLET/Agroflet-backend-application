using System.ComponentModel.DataAnnotations;
namespace Agroflet.Platform.Contact.Interfaces.Rest.Resources;

public record CreateContactMessageResource([Required, StringLength(150)] string Name, [Required, EmailAddress, StringLength(254)] string Email, [Required, StringLength(2000)] string Message);
public record ContactReceiptResource(int Id, DateTime ReceivedAt);
