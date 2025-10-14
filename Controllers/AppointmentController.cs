using System;
using Gestion_Hospital.Models;
using Gestion_Hospital.Services;

namespace Gestion_Hospital.Controllers
{
    public class AppointmentController
    {
        private readonly AppointmentService _service;
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;

        public AppointmentController(AppointmentService service, PatientService patientService, DoctorService doctorService)
        {
            _service = service;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        public void Create()
        {
            Console.WriteLine("=== Agendar Cita ===");
            try
            {
                Console.Write("Documento del paciente: "); var pdoc = Console.ReadLine() ?? "";
                var patient = _patientService.GetByDocument(pdoc);
                if (patient == null) { Console.WriteLine("Paciente no encontrado."); return; }

                Console.Write("Nombre del médico: "); var dname = Console.ReadLine() ?? "";
                var doctor = _doctorService.GetAll().Find(d => d.Name.Equals(dname, StringComparison.OrdinalIgnoreCase));
                if (doctor == null) { Console.WriteLine("Médico no encontrado."); return; }

                Console.Write("Fecha y hora (yyyy-MM-dd HH:mm): "); var dateStr = Console.ReadLine() ?? "";
                if (!DateTime.TryParseExact(dateStr, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime start))
                {
                    Console.WriteLine("Formato de fecha inválido.");
                    return;
                }

                Console.Write("Duración en minutos (ej: 30): "); var durStr = Console.ReadLine() ?? "30";
                if (!int.TryParse(durStr, out int duration)) duration = 30;

                var appointment = _service.Schedule(patient.Id, doctor.Id, start, duration);
                Console.WriteLine($"✅ Cita agendada Id:{appointment.Id} para {appointment.Start} con Dr. {doctor.Name}.");
                Console.WriteLine($"Estado email: {appointment.Notes}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        public void Cancel()
        {
            Console.Write("Id de la cita a cancelar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }

            try
            {
                _service.Cancel(id);
                Console.WriteLine("✅ Cita cancelada.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        public void MarkAttended()
        {
            Console.Write("Id de la cita a marcar como atendida: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }

            try
            {
                _service.MarkAttended(id);
                Console.WriteLine("✅ Cita marcada como atendida.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        public void ListByPatient()
        {
            Console.Write("Documento del paciente: ");
            var doc = Console.ReadLine() ?? "";
            var pat = _patientService.GetByDocument(doc);
            if (pat == null) { Console.WriteLine("Paciente no encontrado."); return; }

            var list = _service.GetByPatient(pat.Id);
            Console.WriteLine($"=== Citas de {pat.Name} ===");
            foreach (var a in list)
            {
                Console.WriteLine($"Id:{a.Id} | {a.Start} - {a.End} | Dr:{a.Doctor?.Name ?? "N/A"} | Estado:{a.Status}");
            }
        }

        public void ListByDoctor()
        {
            Console.Write("Nombre del médico: ");
            var doc = Console.ReadLine() ?? "";
            var docObj = _doctorService.GetAll().Find(d => d.Name.Equals(doc, StringComparison.OrdinalIgnoreCase));
            if (docObj == null) { Console.WriteLine("Médico no encontrado."); return; }

            var list = _service.GetByDoctor(docObj.Id);
            Console.WriteLine($"=== Citas del Dr. {docObj.Name} ===");
            foreach (var a in list)
            {
                Console.WriteLine($"Id:{a.Id} | {a.Start} - {a.End} | PacienteId:{a.PatientId} | Estado:{a.Status}");
            }
        }

        public void Delete()
        {
            Console.WriteLine("=== Eliminar Cita ===");
            Console.Write("Id de la cita a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }
            try
            {
                _service.DeleteAppointment(id);
                Console.WriteLine($"Cita eliminada: Id {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}
