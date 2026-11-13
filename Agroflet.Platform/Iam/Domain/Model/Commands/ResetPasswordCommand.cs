namespace Agroflet.Platform.Iam.Domain.Model.Commands;

public record ResetPasswordCommand(string Token, string NewPassword);
