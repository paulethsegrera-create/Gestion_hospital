using Gestion_Hospital.Interfaces; // Importa las interfaces definidas en el proyecto, como IRegistrable

namespace Gestion_Hospital.Models // Define el namespace donde se encuentran los modelos de datos del hospital
{
    /// <summary>
    /// Represents a doctor in the system. Inherits from <see cref="Person"/>
    /// and implements <see cref="Gestion_Hospital.Interfaces.IRegistrable"/>.
    /// </summary>
    public class Doctor: Person, IRegistrable
    {
    /// <summary>Medical specialty (e.g. Cardiology).</summary>
        public string Specialty { get; set; } = "";
    }
}
