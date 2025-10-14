using System;
using System.Collections.Generic;
using Gestion_Hospital.Interfaces;
using Gestion_Hospital.Models;
using Microsoft.Extensions.Logging;

namespace Gestion_Hospital.Services
{
        public class AppointmentService : Gestion_Hospital.Interfaces.IServiceable
        {
            private readonly IAppointmentRepository _repo;
            private readonly IPatientRepository _patientRepo;
            private readonly IDoctorRepository _doctorRepo;
            private readonly IEmailService _emailService;
            private readonly Microsoft.Extensions.Logging.ILogger<AppointmentService>? _logger;

            public AppointmentService(IAppointmentRepository repo, IPatientRepository patientRepo, IDoctorRepository doctorRepo, IEmailService emailService, Microsoft.Extensions.Logging.ILogger<AppointmentService>? logger = null)
            {
                _repo = repo;
                _patientRepo = patientRepo;
                _doctorRepo = doctorRepo;
                _emailService = emailService;
                _logger = logger;
            }

            public void DeleteAppointment(int appointmentId)
            {
                var a = _repo.GetById(appointmentId) ?? throw new InvalidOperationException("Cita no encontrada.");
                _repo.Delete(appointmentId);
                _logger?.LogInformation("Cita eliminada: {AppointmentId}", appointmentId);
            }

        // duration en minutos (por ejemplo 30)
        public Appointment Schedule(int patientId, int doctorId, DateTime start, int durationMinutes = 30)
        {
            var patient = _patientRepo.GetById(patientId) ?? throw new InvalidOperationException("Paciente no existe.");
            var doctor = _doctorRepo.GetById(doctorId) ?? throw new InvalidOperationException("Médico no existe.");

            if (start < DateTime.Now) throw new InvalidOperationException("No se puede agendar en el pasado.");

            var end = start.AddMinutes(durationMinutes);

            // Conflictos
            if (_repo.ExistsConflictForDoctor(doctorId, start, end)) throw new InvalidOperationException("El médico tiene otra cita en ese horario.");
            if (_repo.ExistsConflictForPatient(patientId, start, end)) throw new InvalidOperationException("El paciente tiene otra cita en ese horario.");

            var appointment = new Appointment
            {
                PatientId = patientId,
                Patient = patient,
                DoctorId = doctorId,
                Doctor = doctor,
                Start = start,
                End = end,
                Status = AppointmentStatus.Scheduled
            };

            _repo.Add(appointment);
            _logger?.LogInformation("Cita agendada: {AppointmentId} paciente:{PatientId} medico:{DoctorId} inicio:{Start}", appointment.Id, patientId, doctorId, start);

            // Envío de correo simulado y registro en historial
            string subject = $"Confirmación de cita con Dr. {doctor.Name}";
            string body = $"Paciente: {patient.Name}\nFecha: {start}\nDoctor: {doctor.Name} ({doctor.Specialty})";
            var emailHistory = _emailService.SendEmail(patient.Email, subject, body);
            // opcional: guardar info del envío en Notes
            appointment.Notes = $"Email status: {(emailHistory.Sent ? "Enviado" : "No enviado")}";
            _repo.Update(appointment);
            _logger?.LogInformation("Email enviado: {Sent} para cita {AppointmentId}", emailHistory.Sent, appointment.Id);

            return appointment;
        }

        public void Cancel(int appointmentId)
        {
            var a = _repo.GetById(appointmentId) ?? throw new InvalidOperationException("Cita no encontrada.");
            a.Status = AppointmentStatus.Cancelled;
            _repo.Update(a);
            _logger?.LogInformation("Cita cancelada: {AppointmentId}", appointmentId);
        }

        public void MarkAttended(int appointmentId)
        {
            var a = _repo.GetById(appointmentId) ?? throw new InvalidOperationException("Cita no encontrada.");
            a.Status = AppointmentStatus.Attended;
            _repo.Update(a);
            _logger?.LogInformation("Cita marcada como atendida: {AppointmentId}", appointmentId);
        }

        public List<Appointment> GetByPatient(int patientId) => _repo.GetByPatient(patientId);
        public List<Appointment> GetByDoctor(int doctorId) => _repo.GetByDoctor(doctorId);
        public List<Appointment> GetAll() => _repo.GetAll();
    }
}
