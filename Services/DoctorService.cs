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
            if (existing == null) throw new InvalidOperationException("Médico no encontrado.");
            // Eliminar citas asociadas
            var citas = _appointmentService.GetByDoctor(id);
            foreach (var cita in citas)
                _appointmentService.DeleteAppointment(cita.Id);
            _repo.Delete(id);
            _logger?.LogInformation("Médico eliminado: {DocumentMasked}", existing.MaskDocument());
        }

        public void RegisterDoctor(Doctor doctor)
        {
            if (string.IsNullOrWhiteSpace(doctor.Name)) throw new ArgumentException("Nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(doctor.Document)) throw new ArgumentException("Documento es obligatorio.");
            if (string.IsNullOrWhiteSpace(doctor.Specialty)) throw new ArgumentException("Especialidad es obligatoria.");

            // Documento único
            if (_repo.GetByDocument(doctor.Document) != null) throw new InvalidOperationException("Ya existe un médico con ese documento.");

            // Validar que no exista la misma combinación nombre+especialidad
            var existsSameNameSpecialty = _repo.GetAll().Any(d => d.Name.ToLower() == doctor.Name.ToLower()
                                                                 && d.Specialty.ToLower() == doctor.Specialty.ToLower());
            if (existsSameNameSpecialty) throw new InvalidOperationException("Ya existe un médico con el mismo nombre y especialidad.");

            _repo.Add(doctor);
            _logger?.LogInformation("Médico registrado: {DocumentMasked} - {Specialty}", doctor.MaskDocument(), doctor.Specialty);
        }

        public void UpdateDoctor(Doctor doctor)
        {
            var existing = _repo.GetById(doctor.Id);
            if (existing == null) throw new InvalidOperationException("Médico no encontrado.");

            var byDoc = _repo.GetByDocument(doctor.Document);
            if (byDoc != null && byDoc.Id != doctor.Id) throw new InvalidOperationException("Otro médico usa ese documento.");

            // Check name+specialty uniqueness
            var clash = _repo.GetAll().Any(d => d.Id != doctor.Id &&
                                                d.Name.ToLower() == doctor.Name.ToLower() &&
                                                d.Specialty.ToLower() == doctor.Specialty.ToLower());
            if (clash) throw new InvalidOperationException("Ya existe otro médico con el mismo nombre y especialidad.");

            _repo.Update(doctor);
        }

        public List<Doctor> GetAll() => _repo.GetAll();
        public List<Doctor> GetBySpecialty(string specialty) => _repo.GetBySpecialty(specialty);
        public Doctor? GetById(int id) => _repo.GetById(id);
    }
}
