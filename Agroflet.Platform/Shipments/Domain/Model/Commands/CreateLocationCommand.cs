namespace Agroflet.Platform.Shipments.Domain.Model.Commands;

public record CreateLocationCommand(string Name, string City, string Region, double Latitude, double Longitude, string Kind);
