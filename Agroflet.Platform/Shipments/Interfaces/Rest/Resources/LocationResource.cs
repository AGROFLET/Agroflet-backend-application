namespace Agroflet.Platform.Shipments.Interfaces.Rest.Resources;

public record LocationResource(int Id, string Name, string City, string Region, double Latitude, double Longitude, string Kind);
