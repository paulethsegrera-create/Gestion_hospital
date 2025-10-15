using System.Collections.Generic;
using System.Linq;
using Gestion_Hospital.Interfaces;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Repositories
{
    /// <summary>
    /// In-memory repository for <see cref="Doctor"/>.
    /// Provides CRUD operations and specialty-based searches.
    /// </summary>
    public class DoctorRepository : IDoctorRepository
    {
        private readonly List<Doctor> _doctors = new();
        private int _nextId = 1;

    /// <summary>Adds a new doctor and assigns Id automatically.</summary>
        public void Add(Doctor doctor)
        {
            doctor.Id = _nextId++;
            _doctors.Add(doctor);
        }

    /// <summary>Updates an existing doctor.</summary>
        public void Update(Doctor doctor)
        {
            var existing = GetById(doctor.Id);
            if (existing == null) return;
            existing.Name = doctor.Name;
            existing.Document = doctor.Document;
            existing.PhoneNumber = doctor.PhoneNumber;
            existing.Email = doctor.Email;
            existing.Specialty = doctor.Specialty;
        }

    /// <summary>Deletes a doctor by Id.</summary>
        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null) _doctors.Remove(existing);
        }

    /// <summary>Gets a doctor by Id.</summary>
        public Doctor? GetById(int id) => _doctors.FirstOrDefault(d => d.Id == id);

    /// <summary>Finds a doctor by document.</summary>
        public Doctor? GetByDocument(string document) => _doctors.FirstOrDefault(d => d.Document == document);

    /// <summary>Returns all doctors.</summary>
        public List<Doctor> GetAll() => _doctors.ToList();

    /// <summary>Filters doctors by specialty (contains, case-insensitive).</summary>
        public List<Doctor> GetBySpecialty(string specialty)
            => _doctors.Where(d => d.Specialty.ToLower().Contains(specialty.ToLower())).ToList();
    }
}
