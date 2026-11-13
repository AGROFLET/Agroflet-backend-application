using Agroflet.Platform.Shipments.Domain.Model.Commands;
namespace Agroflet.Platform.Shipments.Domain.Model.Aggregates;

public partial class Location : Entity
{
    protected Location() { }
    public string Name { get; private set; } = "";
    public string City { get; private set; } = "";
    public string Region { get; private set; } = "";
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string Kind { get; private set; } = "";
    public Location(CreateLocationCommand command)
    {
        Name = command.Name;
        City = command.City;
        Region = command.Region;
        Latitude = command.Latitude;
        Longitude = command.Longitude;
        Kind = command.Kind;
        Validate();
    }
    private void Validate() { }


}
