using System;
using System.Collections.Generic;
using Gestion_Hospital.Interfaces;
using Gestion_Hospital.Models;
using Microsoft.Extensions.Logging;

namespace Gestion_Hospital.Services
{
    public class PatientService : Gestion_Hospital.Interfaces.IServiceable
    {
        private readonly IPatientRepository _repo;
        private readonly Microsoft.Extensions.Logging.ILogger<PatientService>? _logger;
        private readonly AppointmentService _appointmentService;

        public PatientService(IPatientRepository repo, AppointmentService appointmentService, Microsoft.Extensions.Logging.ILogger<PatientService>? logger = null)
        {
            _repo = repo;
            _appointmentService = appointmentService;
            _logger = logger;
        }

        public void DeletePatient(int id)
        {
            var existing = _repo.GetById(id);
            if (existing == null) throw new InvalidOperationException("Patient not found.");
            // Delete associated appointments
            var appointments = _appointmentService.GetByPatient(id);
            foreach (var appointment in appointments)
                _appointmentService.DeleteAppointment(appointment.Id);
            _repo.Delete(id);
            _logger?.LogInformation("Patient deleted: {DocumentMasked}", existing.MaskDocument());
        }

        public void RegisterPatient(Patient patient)
        {
            if (string.IsNullOrWhiteSpace(patient.Name)) throw new ArgumentException("Name is required.");
            if (string.IsNullOrWhiteSpace(patient.Document)) throw new ArgumentException("Document is required.");
            if (_repo.GetByDocument(patient.Document) != null) throw new InvalidOperationException("A patient with that document already exists.");

            _repo.Add(patient);
            _logger?.LogInformation("Patient registered: {DocumentMasked}", patient.MaskDocument());
        }

        public void UpdatePatient(Patient patient)
        {
            var existing = _repo.GetById(patient.Id);
            if (existing == null) throw new InvalidOperationException("Patient not found.");
            // If the document changes, validate uniqueness
            var byDoc = _repo.GetByDocument(patient.Document);
            if (byDoc != null && byDoc.Id != patient.Id) throw new InvalidOperationException("Another patient uses that document.");

            _repo.Update(patient);
        }

        public List<Patient> GetAll() => _repo.GetAll();
        public Patient? GetByDocument(string doc) => _repo.GetByDocument(doc);
        public Patient? GetById(int id) => _repo.GetById(id);
    }
}
