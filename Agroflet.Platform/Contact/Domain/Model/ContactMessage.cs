namespace Agroflet.Platform.Contact.Domain.Model;

public class ContactMessage
{
    public int Id { get; private set; }
    public string Name { get; private set; } = "";
    public string Email { get; private set; } = "";
    public string Message { get; private set; } = "";
    public DateTime ReceivedAt { get; private set; }
    protected ContactMessage() { }
    public ContactMessage(string name, string email, string message)
    {
        Name = Guard.Text(name, "name", 150); Email = Guard.Text(email, "email", 254).ToLowerInvariant();
        Guard.Require(new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(Email), "email-invalid");
        Message = Guard.Text(message, "message", 2000); ReceivedAt = DateTime.UtcNow;
    }
}
