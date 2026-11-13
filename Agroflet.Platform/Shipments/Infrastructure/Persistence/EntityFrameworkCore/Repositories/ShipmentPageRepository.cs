using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Agroflet.Platform.Shipments.Domain.Repositories;
using Agroflet.Platform.Shipments.Domain.Model.Queries;
using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
namespace Agroflet.Platform.Shipments.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ShipmentPageRepository(AppDbContext db) : IShipmentPageRepository
{
    public async Task<ShipmentPage> Find(GetShipmentPageQuery query, int userId, CancellationToken ct)
    {
        var f = query.Filters;
        string? Value(string k) => f.GetValueOrDefault(k)?.FirstOrDefault();
        int Positive(string k, int fallback, int max) { var v = Value(k); if (v == null) return fallback; if (!int.TryParse(v, out var n) || n < 1 || n > max) throw new BusinessException("invalid-pagination"); return n; }
        var allowed = new[] { "page", "currentPage", "pageSize", "cargoType", "status", "destination", "startDate", "endDate", "search", "dispatcherId", "buyerId", "id", "_sort", "_order" };
        if (f.Keys.Any(k => !allowed.Contains(k))) throw new BusinessException("unsupported-filter");
        var page = Positive(f.ContainsKey("currentPage") ? "currentPage" : "page", 1, int.MaxValue); var size = Positive("pageSize", 20, 100);
        var q = db.Set<Shipment>().AsNoTracking().Where(s => s.DispatcherId == userId || s.BuyerId == userId);
        if (Value("cargoType") is string cargo) q = q.Where(s => s.CargoType == cargo);
        if (Value("status") is string status) { Guard.OneOf(status, "planned", "in_transit", "delivered", "cancelled"); q = q.Where(s => s.Status == status); }
        foreach (var key in new[] { "dispatcherId", "buyerId", "id" }) if (f.TryGetValue(key, out var values))
        { var ids = values.Select(v => int.TryParse(v, out var id) && id > 0 ? id : throw new BusinessException("invalid-id")).ToArray(); q = key switch { "dispatcherId" => q.Where(s => ids.Contains(s.DispatcherId)), "buyerId" => q.Where(s => ids.Contains(s.BuyerId)), _ => q.Where(s => ids.Contains(s.Id)) }; }
        DateTime? Date(string k) { if (Value(k) is not string v) return null; if (!DateTimeOffset.TryParse(v, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var d)) throw new BusinessException("invalid-date"); return d.UtcDateTime; }
        var start = Date("startDate"); var end = Date("endDate"); if (start > end) throw new BusinessException("inverted-date-range");
        if (start.HasValue) q = q.Where(s => s.PlannedDepartureAt >= start.Value);
        if (end.HasValue) { var limit = end.Value; if (Value("endDate")!.Length == 10) { limit = limit.AddDays(1); q = q.Where(s => s.PlannedDepartureAt < limit); } else q = q.Where(s => s.PlannedDepartureAt <= limit); }
        if (Value("destination") is string destination)
        { if (int.TryParse(destination, out var id)) q = q.Where(s => s.DestinationId == id); else q = q.Where(s => db.Set<Location>().Any(l => l.Id == s.DestinationId && (l.Name.Contains(destination) || l.City.Contains(destination)))); }
        if (Value("search") is string term && !string.IsNullOrWhiteSpace(term)) { term = term.Trim().ToUpperInvariant(); q = q.Where(s => s.Code.Contains(term) || db.Set<Vehicle>().Any(v => v.Id == s.VehicleId && v.Plate.Contains(term))); }
        var total = await q.CountAsync(ct); var pages = (int)Math.Ceiling(total / (double)size); var offset = ((long)page - 1) * size;
        if (offset > int.MaxValue) throw new BusinessException("invalid-pagination");
        var descending = Value("_order") != "asc";
        q = (Value("_sort") ?? "plannedDepartureAt") switch
        {
            "id" => descending ? q.OrderByDescending(s => s.Id) : q.OrderBy(s => s.Id),
            "plannedDepartureAt" => descending ? q.OrderByDescending(s => s.PlannedDepartureAt).ThenByDescending(s => s.Id) : q.OrderBy(s => s.PlannedDepartureAt).ThenBy(s => s.Id),
            _ => throw new BusinessException("unsupported-sort")
        };
        return new(await q.Skip((int)offset).Take(size).ToListAsync(ct), total, page, size, pages);
    }
}
