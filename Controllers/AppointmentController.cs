using System;
using Gestion_Hospital.Models;
using Gestion_Hospital.Services;

namespace Gestion_Hospital.Controllers
{
    // Controlador encargado de manejar las operaciones relacionadas con las citas médicas.
    public class AppointmentController
    {
        // Dependencias principales (inyección manual de servicios)
        private readonly AppointmentService _service;
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;

        // Constructor: recibe los servicios que necesita para funcionar.
        public AppointmentController(AppointmentService service, PatientService patientService, DoctorService doctorService)
        {
            _service = service;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        // Método para crear (agendar) una nueva cita médica.
        public void Create()
        {
            Console.WriteLine("=== Agendar Cita ===");
            try
            {
                // Solicita el documento del paciente para buscarlo en el sistema.
                Console.Write("Documento del paciente: "); var pdoc = Console.ReadLine() ?? "";
                var patient = _patientService.GetByDocument(pdoc);
                if (patient == null) { Console.WriteLine("Paciente no encontrado."); return; }

                // Solicita el nombre del médico para asociar la cita.
                Console.Write("Nombre del médico: "); var dname = Console.ReadLine() ?? "";
                var doctor = _doctorService.GetAll().Find(d => d.Name.Equals(dname, StringComparison.OrdinalIgnoreCase));
                if (doctor == null) { Console.WriteLine("Médico no encontrado."); return; }

                // Solicita la fecha y hora de inicio de la cita.
                Console.Write("Fecha y hora (yyyy-MM-dd HH:mm): "); var dateStr = Console.ReadLine() ?? "";
                if (!DateTime.TryParseExact(dateStr, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime start))
                {
                    Console.WriteLine("Formato de fecha inválido.");
                    return;
                }

                // Solicita la duración de la cita (en minutos). Si no se ingresa, usa 30 como valor por defecto.
                Console.Write("Duración en minutos (ej: 30): "); var durStr = Console.ReadLine() ?? "30";
                if (!int.TryParse(durStr, out int duration)) duration = 30;

                // Llama al servicio de citas para agendarla.
                var appointment = _service.Schedule(patient.Id, doctor.Id, start, duration);

                // Muestra confirmación al usuario.
                Console.WriteLine($"✅ Cita agendada Id:{appointment.Id} para {appointment.Start} con Dr. {doctor.Name}.");
                Console.WriteLine($"Estado email: {appointment.Notes}");
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción y muestra el error.
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        // Método para cancelar una cita existente.
        public void Cancel()
        {
            Console.Write("Id de la cita a cancelar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }

            try
            {
                // Llama al servicio para cancelar la cita por Id.
                _service.Cancel(id);
                Console.WriteLine("✅ Cita cancelada.");
            }
            catch (Exception ex)
            {
                // Muestra errores en caso de fallo.
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        // Método para marcar una cita como atendida.
        public void MarkAttended()
        {
            Console.Write("Id de la cita a marcar como atendida: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }

            try
            {
                // Llama al servicio para actualizar el estado a “Atendida”.
                _service.MarkAttended(id);
                Console.WriteLine("✅ Cita marcada como atendida.");
            }
            catch (Exception ex)
            {
                // Muestra cualquier error.
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        // Método para listar todas las citas de un paciente.
        public void ListByPatient()
        {
            Console.Write("Documento del paciente: ");
            var doc = Console.ReadLine() ?? "";
            var pat = _patientService.GetByDocument(doc);
            if (pat == null) { Console.WriteLine("Paciente no encontrado."); return; }

            // Obtiene todas las citas asociadas a ese paciente.
            var list = _service.GetByPatient(pat.Id);
            Console.WriteLine($"=== Citas de {pat.Name} ===");
            foreach (var a in list)
            {
                // Muestra cada cita con su información relevante.
                Console.WriteLine($"Id:{a.Id} | {a.Start} - {a.End} | Dr:{a.Doctor?.Name ?? "N/A"} | Estado:{a.Status}");
            }
        }

        // Método para listar todas las citas de un médico.
        public void ListByDoctor()
        {
            Console.Write("Nombre del médico: ");
            var doc = Console.ReadLine() ?? "";
            var docObj = _doctorService.GetAll().Find(d => d.Name.Equals(doc, StringComparison.OrdinalIgnoreCase));
            if (docObj == null) { Console.WriteLine("Médico no encontrado."); return; }

            // Obtiene todas las citas asociadas al médico.
            var list = _service.GetByDoctor(docObj.Id);
            Console.WriteLine($"=== Citas del Dr. {docObj.Name} ===");
            foreach (var a in list)
            {
                // Muestra cada cita con su información relevante.
                Console.WriteLine($"Id:{a.Id} | {a.Start} - {a.End} | PacienteId:{a.PatientId} | Estado:{a.Status}");
            }
        }

        // Método para eliminar una cita del sistema.
        public void Delete()
        {
            Console.WriteLine("=== Eliminar Cita ===");
            Console.Write("Id de la cita a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }
            try
            {
                // Llama al servicio para borrar la cita permanentemente.
                _service.DeleteAppointment(id);
                Console.WriteLine($"Cita eliminada: Id {id}");
            }
            catch (Exception ex)
            {
                // Maneja errores si algo sale mal.
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}
