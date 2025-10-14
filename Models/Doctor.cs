using Gestion_Hospital.Interfaces; // Importa las interfaces definidas en el proyecto, como IRegistrable

namespace Gestion_Hospital.Models // Define el namespace donde se encuentran los modelos de datos del hospital
{
    /// <summary>
    /// Representa a un médico en el sistema. Hereda de <see cref="Person"/>
    /// e implementa <see cref="Gestion_Hospital.Interfaces.IRegistrable"/>.
    /// </summary>
    public class Doctor: Person, IRegistrable
    {
        /// <summary>Especialidad médica (ej. Cardiología).</summary>
        public string Specialty { get; set; } = "";
    }
}
