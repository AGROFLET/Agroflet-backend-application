namespace Agroflet.Platform.Shared.Domain.Model;

/// <summary>MySQL DATETIME(6) precision, also used for idempotent payload comparisons.</summary>
public static class UtcTimestamp
{
    public static DateTime Normalize(DateTime value)
    {
        var utc = value.ToUniversalTime();
        return new DateTime(utc.Ticks - utc.Ticks % 10, DateTimeKind.Utc);
    }
}
