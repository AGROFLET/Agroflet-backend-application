using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
namespace Agroflet.Platform.Shipments.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyShipmentsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Shipment>().HasBaseType((Type?)null);
        builder.Entity<Shipment>().HasKey(e => e.Id);
        builder.Entity<Shipment>().Property(e => e.Version).IsConcurrencyToken();
        builder.Entity<Location>().HasBaseType((Type?)null);
        builder.Entity<Location>().HasKey(e => e.Id);
        builder.Entity<Location>().Property(e => e.Version).IsConcurrencyToken();
        builder.Entity<StatusChange>().HasBaseType((Type?)null);
        builder.Entity<StatusChange>().HasKey(e => e.Id);
        builder.Entity<StatusChange>().Property(e => e.Version).IsConcurrencyToken();
        builder.Entity<Shipment>().HasIndex(e => e.Code).IsUnique();
        builder.Entity<Shipment>().HasOne<User>().WithMany().HasForeignKey(e => e.DispatcherId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Shipment>().HasOne<User>().WithMany().HasForeignKey(e => e.BuyerId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Shipment>().HasOne<Vehicle>().WithMany().HasForeignKey(e => e.VehicleId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Shipment>().HasOne<Driver>().WithMany().HasForeignKey(e => e.DriverId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Shipment>().HasOne<Location>().WithMany().HasForeignKey(e => e.OriginId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Shipment>().HasOne<Location>().WithMany().HasForeignKey(e => e.DestinationId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<StatusChange>().HasOne<Shipment>().WithMany().HasForeignKey(e => e.ShipmentId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<StatusChange>().HasOne<User>().WithMany().HasForeignKey(e => e.ChangedBy).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Location>().HasData(
            new { Id = 1, Version = 0, Name = "Centro de Acopio El Tambo", City = "Huancayo", Region = "Junín", Latitude = -12.056d, Longitude = -75.216d, Kind = "origin" },
            new { Id = 2, Version = 0, Name = "Acopio Amarilis", City = "Huánuco", Region = "Huánuco", Latitude = -9.9306d, Longitude = -76.2422d, Kind = "origin" },
            new { Id = 3, Version = 0, Name = "Fundo Los Aquijes", City = "Ica", Region = "Ica", Latitude = -14.094d, Longitude = -75.705d, Kind = "origin" },
            new { Id = 4, Version = 0, Name = "Acopio Laredo", City = "Trujillo", Region = "La Libertad", Latitude = -8.0891d, Longitude = -78.96d, Kind = "origin" },
            new { Id = 5, Version = 0, Name = "Acopio San Vicente", City = "Cañete", Region = "Lima Provincias", Latitude = -13.077d, Longitude = -76.387d, Kind = "origin" },
            new { Id = 6, Version = 0, Name = "Acopio Valle de Huaral", City = "Huaral", Region = "Lima Provincias", Latitude = -11.495d, Longitude = -77.207d, Kind = "origin" },
            new { Id = 7, Version = 0, Name = "Gran Mercado Mayorista de Lima", City = "Santa Anita", Region = "Lima Metropolitana", Latitude = -12.056d, Longitude = -76.969d, Kind = "destination" },
            new { Id = 8, Version = 0, Name = "Mercado Mayorista N.° 2 de Frutas", City = "La Victoria", Region = "Lima Metropolitana", Latitude = -12.068d, Longitude = -77.019d, Kind = "destination" },
            new { Id = 9, Version = 0, Name = "Centro de Distribución Lurín", City = "Lurín", Region = "Lima Metropolitana", Latitude = -12.274d, Longitude = -76.87d, Kind = "destination" },
            new { Id = 10, Version = 0, Name = "Mercado de Productores de Santa Anita", City = "Santa Anita", Region = "Lima Metropolitana", Latitude = -12.048d, Longitude = -76.962d, Kind = "destination" }
        );
    }
}
