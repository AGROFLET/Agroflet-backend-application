using System.Net;
using System.Net.Mail;
using Agroflet.Platform.Iam.Application.CommandServices;
using Agroflet.Platform.Iam.Application.QueryServices;
using Agroflet.Platform.Iam.Application.Internal.OutboundServices;
namespace Agroflet.Platform.Iam.Infrastructure.Email;

public class PasswordResetDelivery(IConfiguration config, IHostEnvironment environment) : IPasswordResetDelivery
{
    public async Task<string?> Deliver(string email, string token, CancellationToken ct)
    {
        if (environment.IsDevelopment() && config.GetValue<bool>("PasswordReset:ReturnTokenInDevelopment")) return token;
        if (string.IsNullOrWhiteSpace(config["Smtp:Host"])) throw new InvalidOperationException("SMTP is not configured.");
        using var client = new SmtpClient(config["Smtp:Host"], config.GetValue<int>("Smtp:Port", 587)) { EnableSsl = true, Credentials = new NetworkCredential(config["Smtp:User"], config["Smtp:Password"]) };
        using var message = new MailMessage(config["Smtp:From"]!, email, "AgroFlet: recuperar contraseña", config["PasswordReset:FrontendUrl"] + "?token=" + Uri.EscapeDataString(token));
        await client.SendMailAsync(message, ct); return null;
    }
}
