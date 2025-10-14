using System.Collections.Generic;
using System.Linq;
using Gestion_Hospital.Interfaces;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly List<Doctor> _doctors = new();
        private int _nextId = 1;

        public void Add(Doctor doctor)
        {
            doctor.Id = _nextId++;
            _doctors.Add(doctor);
        }

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

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null) _doctors.Remove(existing);
        }

        public Doctor? GetById(int id) => _doctors.FirstOrDefault(d => d.Id == id);
        public Doctor? GetByDocument(string document) => _doctors.FirstOrDefault(d => d.Document == document);
        public List<Doctor> GetAll() => _doctors.ToList();
        public List<Doctor> GetBySpecialty(string specialty)
            => _doctors.Where(d => d.Specialty.ToLower().Contains(specialty.ToLower())).ToList();
    }
}
