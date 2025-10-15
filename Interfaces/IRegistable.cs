namespace Gestion_Hospital.Interfaces // Defines the namespace to organize hospital interfaces
{
    /// <summary>
    /// Simple contract to mark entities as registrable/persistable in the system.
    /// Implementations must expose a <see cref="Id"/> property.
    /// </summary>
    public interface IRegistrable
    {
    /// <summary>Unique identifier of the entity.</summary>
        int Id { get; set; }
    }
}
