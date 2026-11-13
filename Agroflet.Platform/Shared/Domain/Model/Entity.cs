namespace Agroflet.Platform.Shared.Domain.Model;

public abstract class Entity
{
    public int Id { get; protected set; }
    public int Version { get; private set; }
    public void AdvanceVersion() => Version++;
}
