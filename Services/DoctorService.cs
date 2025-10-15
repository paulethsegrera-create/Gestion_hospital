using System;
using System.Collections.Generic;
using System.Linq;
using Gestion_Hospital.Interfaces;
using Gestion_Hospital.Models;
using Microsoft.Extensions.Logging;

namespace Gestion_Hospital.Services
{
    public class DoctorService : Gestion_Hospital.Interfaces.IServiceable
    {
        private readonly IDoctorRepository _repo;
        private readonly Microsoft.Extensions.Logging.ILogger<DoctorService>? _logger;
        private readonly AppointmentService _appointmentService;

        public DoctorService(IDoctorRepository repo, AppointmentService appointmentService, Microsoft.Extensions.Logging.ILogger<DoctorService>? logger = null)
        {
            _repo = repo;
            _appointmentService = appointmentService;
            _logger = logger;
        }

        public void DeleteDoctor(int id)
        {
            var existing = _repo.GetById(id);
            if (existing == null) throw new InvalidOperationException("Doctor not found.");
            // Delete associated appointments
            var appointments = _appointmentService.GetByDoctor(id);
            foreach (var appointment in appointments)
                _appointmentService.DeleteAppointment(appointment.Id);
            _repo.Delete(id);
            _logger?.LogInformation("Doctor deleted: {DocumentMasked}", existing.MaskDocument());
        }

        public void RegisterDoctor(Doctor doctor)
        {
            if (string.IsNullOrWhiteSpace(doctor.Name)) throw new ArgumentException("Name is required.");
            if (string.IsNullOrWhiteSpace(doctor.Document)) throw new ArgumentException("Document is required.");
            if (string.IsNullOrWhiteSpace(doctor.Specialty)) throw new ArgumentException("Specialty is required.");

            // Unique document
            if (_repo.GetByDocument(doctor.Document) != null) throw new InvalidOperationException("A doctor with that document already exists.");

            // Validate that the same name+specialty combination does not exist
            var existsSameNameSpecialty = _repo.GetAll().Any(d => d.Name.ToLower() == doctor.Name.ToLower()
                                                                 && d.Specialty.ToLower() == doctor.Specialty.ToLower());
            if (existsSameNameSpecialty) throw new InvalidOperationException("A doctor with the same name and specialty already exists.");

            _repo.Add(doctor);
            _logger?.LogInformation("Doctor registered: {DocumentMasked} - {Specialty}", doctor.MaskDocument(), doctor.Specialty);
        }

        public void UpdateDoctor(Doctor doctor)
        {
            var existing = _repo.GetById(doctor.Id);
            if (existing == null) throw new InvalidOperationException("Doctor not found.");

            var byDoc = _repo.GetByDocument(doctor.Document);
            if (byDoc != null && byDoc.Id != doctor.Id) throw new InvalidOperationException("Another doctor uses that document.");

            // Check name+specialty uniqueness
            var clash = _repo.GetAll().Any(d => d.Id != doctor.Id &&
                                                d.Name.ToLower() == doctor.Name.ToLower() &&
                                                d.Specialty.ToLower() == doctor.Specialty.ToLower());
            if (clash) throw new InvalidOperationException("Another doctor with the same name and specialty already exists.");

            _repo.Update(doctor);
        }

        public List<Doctor> GetAll() => _repo.GetAll();
        public List<Doctor> GetBySpecialty(string specialty) => _repo.GetBySpecialty(specialty);
        public Doctor? GetById(int id) => _repo.GetById(id);
    }
}
