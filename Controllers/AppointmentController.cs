using System;
using Gestion_Hospital.Models;
using Gestion_Hospital.Services;

namespace Gestion_Hospital.Controllers
{
    // Controller responsible for handling operations related to medical appointments.
    public class AppointmentController
    {
    // Main dependencies (manual service injection)
        private readonly AppointmentService _service;
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;

    // Constructor: receives the services required for operation.
        public AppointmentController(AppointmentService service, PatientService patientService, DoctorService doctorService)
        {
            _service = service;
            _patientService = patientService;
            _doctorService = doctorService;
        }

    // Method to create (schedule) a new medical appointment.
        public void Create()
        {
            Console.WriteLine("=== Agendar Cita ===");
            try
            {
                // Requests the patient's document to search for it in the system.
                Console.Write("Documento del paciente: "); var pdoc = Console.ReadLine() ?? "";
                var patient = _patientService.GetByDocument(pdoc);
                if (patient == null) { Console.WriteLine("Paciente no encontrado."); return; }

                // Requests the doctor's name to associate the appointment.
                Console.Write("Nombre del médico: "); var dname = Console.ReadLine() ?? "";
                var doctor = _doctorService.GetAll().Find(d => d.Name.Equals(dname, StringComparison.OrdinalIgnoreCase));
                if (doctor == null) { Console.WriteLine("Médico no encontrado."); return; }

                // Requests the start date and time of the appointment.
                Console.Write("Fecha y hora (yyyy-MM-dd HH:mm): "); var dateStr = Console.ReadLine() ?? "";
                if (!DateTime.TryParseExact(dateStr, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime start))
                {
                    Console.WriteLine("Formato de fecha inválido.");
                    return;
                }

                // Requests the appointment duration (in minutes). If not entered, 30 is used as the default.
                Console.Write("Duración en minutos (ej: 30): "); var durStr = Console.ReadLine() ?? "30";
                if (!int.TryParse(durStr, out int duration)) duration = 30;

                // Calls the appointment service to schedule it.
                var appointment = _service.Schedule(patient.Id, doctor.Id, start, duration);

                // Shows confirmation to the user.
                Console.WriteLine($"✅ Cita agendada Id:{appointment.Id} para {appointment.Start} con Dr. {doctor.Name}.");
                Console.WriteLine($"Estado email: {appointment.Notes}");
            }
            catch (Exception ex)
            {
                // Catches any exceptions and displays the error.
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

    // Method to cancel an existing appointment.
        public void Cancel()
        {
            Console.Write("Id de la cita a cancelar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }

            try
            {
                // Calls the service to cancel the appointment by Id.
                _service.Cancel(id);
                Console.WriteLine("✅ Cita cancelada.");
            }
            catch (Exception ex)
            {
                // Shows errors in case of failure.
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

    // Method to mark an appointment as attended.
        public void MarkAttended()
        {
            Console.Write("Id de la cita a marcar como atendida: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }

            try
            {
                // Calls the service to update the status to "Attended".
                _service.MarkAttended(id);
                Console.WriteLine("✅ Cita marcada como atendida.");
            }
            catch (Exception ex)
            {
                // Shows any error.
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

    // Method to list all appointments for a patient.
        public void ListByPatient()
        {
            Console.Write("Documento del paciente: ");
            var doc = Console.ReadLine() ?? "";
            var pat = _patientService.GetByDocument(doc);
            if (pat == null) { Console.WriteLine("Paciente no encontrado."); return; }

            // Gets all appointments associated with that patient.
            var list = _service.GetByPatient(pat.Id);
            Console.WriteLine($"=== Citas de {pat.Name} ===");
            foreach (var a in list)
            {
                // Displays each appointment with its relevant information.
                Console.WriteLine($"Id:{a.Id} | {a.Start} - {a.End} | Dr:{a.Doctor?.Name ?? "N/A"} | Estado:{a.Status}");
            }
        }

    // Method to list all of a doctor's appointments.
        public void ListByDoctor()
        {
            Console.Write("Nombre del médico: ");
            var doc = Console.ReadLine() ?? "";
            var docObj = _doctorService.GetAll().Find(d => d.Name.Equals(doc, StringComparison.OrdinalIgnoreCase));
            if (docObj == null) { Console.WriteLine("Médico no encontrado."); return; }

            // Gets all appointments associated with the doctor.
            var list = _service.GetByDoctor(docObj.Id);
            Console.WriteLine($"=== Citas del Dr. {docObj.Name} ===");
            foreach (var a in list)
            {
                // Displays each appointment with its relevant information.
                Console.WriteLine($"Id:{a.Id} | {a.Start} - {a.End} | PacienteId:{a.PatientId} | Estado:{a.Status}");
            }
        }

    // Method to permanently delete an appointment.
        public void Delete()
        {
            Console.WriteLine("=== Eliminar Cita ===");
            Console.Write("Id de la cita a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }
            try
            {
                // Calls the service to permanently delete the appointment.
                _service.DeleteAppointment(id);
                Console.WriteLine($"Cita eliminada: Id {id}");
            }
            catch (Exception ex)
            {
                // Shows any errors encountered.
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}
