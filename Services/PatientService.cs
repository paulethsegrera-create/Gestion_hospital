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
            if (existing == null) throw new InvalidOperationException("Paciente no encontrado.");
            // Eliminar citas asociadas
            var citas = _appointmentService.GetByPatient(id);
            foreach (var cita in citas)
                _appointmentService.DeleteAppointment(cita.Id);
            _repo.Delete(id);
            _logger?.LogInformation("Paciente eliminado: {DocumentMasked}", existing.MaskDocument());
        }

        public void RegisterPatient(Patient patient)
        {
            if (string.IsNullOrWhiteSpace(patient.Name)) throw new ArgumentException("Nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(patient.Document)) throw new ArgumentException("Documento es obligatorio.");
            if (_repo.GetByDocument(patient.Document) != null) throw new InvalidOperationException("Ya existe un paciente con ese documento.");

            _repo.Add(patient);
            _logger?.LogInformation("Paciente registrado: {DocumentMasked}", patient.MaskDocument());
        }

        public void UpdatePatient(Patient patient)
        {
            var existing = _repo.GetById(patient.Id);
            if (existing == null) throw new InvalidOperationException("Paciente no encontrado.");
            // si el documento cambia, validar unicidad
            var byDoc = _repo.GetByDocument(patient.Document);
            if (byDoc != null && byDoc.Id != patient.Id) throw new InvalidOperationException("Otro paciente usa ese documento.");

            _repo.Update(patient);
        }

        public List<Patient> GetAll() => _repo.GetAll();
        public Patient? GetByDocument(string doc) => _repo.GetByDocument(doc);
        public Patient? GetById(int id) => _repo.GetById(id);
    }
}
