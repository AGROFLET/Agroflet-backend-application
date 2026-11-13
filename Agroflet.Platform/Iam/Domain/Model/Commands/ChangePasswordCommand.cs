namespace Agroflet.Platform.Iam.Domain.Model.Commands;

public record ChangePasswordCommand(string CurrentPassword, string NewPassword);
