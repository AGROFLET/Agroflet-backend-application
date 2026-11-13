using System.Globalization;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Shared.Application.Patterns;
/// <summary>Applies json-server compatible filters after mandatory ownership scoping.</summary>
public static class CollectionFilter
{
    public static IReadOnlyList<T> Apply<T>(IEnumerable<T> items, CollectionQuery query)
    {
        var properties = typeof(T).GetProperties();
        foreach (var pair in query.Filters.Where(p => !p.Key.StartsWith('_')))
        {
            var prop = properties.FirstOrDefault(p => string.Equals(p.Name, pair.Key, StringComparison.OrdinalIgnoreCase));
            if (prop == null) throw new ArgumentException("unsupported-filter");
            items = items.Where(x => pair.Value.Any(v => string.Equals(Convert.ToString(prop.GetValue(x), CultureInfo.InvariantCulture), v, StringComparison.OrdinalIgnoreCase)));
        }
        var sort = query.Filters.GetValueOrDefault("_sort")?.FirstOrDefault() ?? "id";
        var sortProperty = properties.FirstOrDefault(p => string.Equals(p.Name, sort, StringComparison.OrdinalIgnoreCase)) ?? throw new ArgumentException("unsupported-sort");
        items = query.Filters.GetValueOrDefault("_order")?.FirstOrDefault() == "desc" ? items.OrderByDescending(x => sortProperty.GetValue(x)) : items.OrderBy(x => sortProperty.GetValue(x));
        if (query.Filters.TryGetValue("_limit", out var values)) { if (!int.TryParse(values.FirstOrDefault(), out var limit) || limit < 1 || limit > 1000) throw new ArgumentException("invalid-limit"); items = items.Take(limit); }
        return items.ToList();
    }
}
