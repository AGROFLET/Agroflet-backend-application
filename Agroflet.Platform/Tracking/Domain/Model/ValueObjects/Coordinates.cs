namespace Agroflet.Platform.Tracking.Domain.Model.ValueObjects;

/// <summary>Geographic coordinates; a reported location is not a continuous GPS feed.</summary>
public sealed record Coordinates
{
    public double Latitude { get; }
    public double Longitude { get; }
    public Coordinates(double latitude, double longitude)
    {
        Guard.Require(double.IsFinite(latitude) && latitude is >= -90 and <= 90, "latitude-range");
        Guard.Require(double.IsFinite(longitude) && longitude is >= -180 and <= 180, "longitude-range");
        Latitude = latitude;
        Longitude = longitude;
    }
}
