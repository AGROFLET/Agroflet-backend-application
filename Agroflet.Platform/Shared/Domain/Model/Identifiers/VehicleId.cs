namespace Agroflet.Platform.Shared.Domain.Model.Identifiers;

public readonly record struct VehicleId
{
    public int Value { get; }
    public VehicleId(int value) { if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value)); Value = value; }
    public override string ToString() => Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
}
