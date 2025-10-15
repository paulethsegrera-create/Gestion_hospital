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
                var a = _repo.GetById(appointmentId) ?? throw new InvalidOperationException("Appointment not found.");
                _repo.Delete(appointmentId);
                _logger?.LogInformation("Appointment deleted: {AppointmentId}", appointmentId);
            }

        // duration in minutes (e.g. 30)
        public Appointment Schedule(int patientId, int doctorId, DateTime start, int durationMinutes = 30)
        {
            var patient = _patientRepo.GetById(patientId) ?? throw new InvalidOperationException("Patient does not exist.");
            var doctor = _doctorRepo.GetById(doctorId) ?? throw new InvalidOperationException("Doctor does not exist.");

            if (start < DateTime.Now) throw new InvalidOperationException("Cannot schedule in the past.");

            var end = start.AddMinutes(durationMinutes);

            // Conflicts
            if (_repo.ExistsConflictForDoctor(doctorId, start, end)) throw new InvalidOperationException("The doctor has another appointment at that time.");
            if (_repo.ExistsConflictForPatient(patientId, start, end)) throw new InvalidOperationException("The patient has another appointment at that time.");

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
            _logger?.LogInformation("Appointment scheduled: {AppointmentId} patient:{PatientId} doctor:{DoctorId} start:{Start}", appointment.Id, patientId, doctorId, start);

            // Simulated email sending and history logging
            string subject = $"Appointment confirmation with Dr. {doctor.Name}";
            string body = $"Patient: {patient.Name}\nDate: {start}\nDoctor: {doctor.Name} ({doctor.Specialty})";
            var emailHistory = _emailService.SendEmail(patient.Email, subject, body);
            // Optionally: save sending info in Notes
            appointment.Notes = $"Email status: {(emailHistory.Sent ? "Sent" : "Not sent")}";
            _repo.Update(appointment);
            _logger?.LogInformation("Email sent: {Sent} for appointment {AppointmentId}", emailHistory.Sent, appointment.Id);

            return appointment;
        }

        public void Cancel(int appointmentId)
        {
            var a = _repo.GetById(appointmentId) ?? throw new InvalidOperationException("Appointment not found.");
            a.Status = AppointmentStatus.Cancelled;
            _repo.Update(a);
            _logger?.LogInformation("Appointment cancelled: {AppointmentId}", appointmentId);
        }

        public void MarkAttended(int appointmentId)
        {
            var a = _repo.GetById(appointmentId) ?? throw new InvalidOperationException("Appointment not found.");
            a.Status = AppointmentStatus.Attended;
            _repo.Update(a);
            _logger?.LogInformation("Appointment marked as attended: {AppointmentId}", appointmentId);
        }

        public List<Appointment> GetByPatient(int patientId) => _repo.GetByPatient(patientId);
        public List<Appointment> GetByDoctor(int doctorId) => _repo.GetByDoctor(doctorId);
        public List<Appointment> GetAll() => _repo.GetAll();
    }
}
