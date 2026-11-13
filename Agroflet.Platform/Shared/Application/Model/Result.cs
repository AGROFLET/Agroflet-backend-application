namespace Agroflet.Platform.Shared.Application.Model;

public sealed record Error(string Code, int Status);

/// <summary>Application outcome independent of HTTP controller types.</summary>
public sealed class Result<T>
{
    private Result(bool success, T? value, Error? error) { IsSuccess = success; Value = value; Error = error; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public Error? Error { get; }
    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(Error error) => new(false, default, error);
    public TResult Match<TResult>(Func<T, TResult> success, Func<Error, TResult> failure) => IsSuccess ? success(Value!) : failure(Error!);
    // Composition of multiple authorized queries preserves the first failed outcome.
    public T RequireValue() => IsSuccess ? Value! : throw new BusinessException(Error!.Code, Error.Status);
    public static async Task<Result<T>> ExecuteAsync(Func<Task<T>> operation)
    {
        try { return Success(await operation()); }
        catch (BusinessException ex) { return Failure(new(ex.Message, ex.Status)); }
        catch (ArgumentException ex) { return Failure(new(ex.Message, 400)); }
    }
}
public readonly record struct Unit;
