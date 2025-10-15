using Gestion_Hospital.Repositories;

namespace Gestion_Hospital.DataBase
{
    /// <summary>
    /// DataContext centraliza el acceso a los repositorios del sistema hospitalario.
    /// Permite compartir instancias y facilita la inyección de dependencias.
    /// </summary>
    public class DataContext
    {
        // Repositorios
        public PatientRepository PatientRepository { get; }
        public DoctorRepository DoctorRepository { get; }
        public AppointmentRepository AppointmentRepository { get; }

        /// <summary>
        /// Inicializa el DataContext con repositorios en memoria.
        /// </summary>
        public DataContext()
        {
            PatientRepository = new PatientRepository();
            DoctorRepository = new DoctorRepository();
            AppointmentRepository = new AppointmentRepository();
        }
    }
}
