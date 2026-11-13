namespace Agroflet.Platform.Shared.Domain.Model.Errors;

public sealed class BusinessException(string code, int status = 400) : Exception(code)
{
    public int Status { get; } = status;
}
