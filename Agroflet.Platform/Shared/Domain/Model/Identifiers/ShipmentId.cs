namespace Agroflet.Platform.Shared.Domain.Model.Identifiers;

public readonly record struct ShipmentId
{
    public int Value { get; }
    public ShipmentId(int value) { if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value)); Value = value; }
    public override string ToString() => Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
}
