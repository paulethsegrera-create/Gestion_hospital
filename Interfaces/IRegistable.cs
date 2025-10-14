namespace Gestion_Hospital.Interfaces // Define el namespace para organizar las interfaces del hospital
{
    /// <summary>
    /// Contrato simple para marcar entidades registrables/persistibles en el sistema.
    /// Las implementaciones deben exponer una propiedad <see cref="Id"/>.
    /// </summary>
    public interface IRegistrable
    {
        /// <summary>Identificador único de la entidad.</summary>
        int Id { get; set; }
    }
}
