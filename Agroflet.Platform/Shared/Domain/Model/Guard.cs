namespace Agroflet.Platform.Shared.Domain.Model;

public static class Guard
{
    public static string Text(string? value, string field, int max = 255)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length > max) throw new ArgumentException(field + "-invalid");
        return value.Trim();
    }
    public static string OneOf(string value, params string[] allowed)
    {
        if (!allowed.Contains(value)) throw new ArgumentException("invalid-value");
        return value;
    }
    public static void Require(bool condition, string code) { if (!condition) throw new ArgumentException(code); }
}
