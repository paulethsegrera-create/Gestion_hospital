using System.Collections.Generic;
using System.Linq;
using Gestion_Hospital.Interfaces;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Repositories
{
    /// <summary>
    /// Repositorio en memoria para <see cref="Doctor"/>.
    /// Proporciona operaciones CRUD y búsquedas por especialidad.
    /// </summary>
    public class DoctorRepository : IDoctorRepository
    {
        private readonly List<Doctor> _doctors = new();
        private int _nextId = 1;

        /// <summary>Agrega un nuevo médico y asigna Id automáticamente.</summary>
        public void Add(Doctor doctor)
        {
            doctor.Id = _nextId++;
            _doctors.Add(doctor);
        }

        /// <summary>Actualiza un médico existente.</summary>
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

        /// <summary>Elimina un médico por Id.</summary>
        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null) _doctors.Remove(existing);
        }

        /// <summary>Obtiene un médico por Id.</summary>
        public Doctor? GetById(int id) => _doctors.FirstOrDefault(d => d.Id == id);

        /// <summary>Busca un médico por documento.</summary>
        public Doctor? GetByDocument(string document) => _doctors.FirstOrDefault(d => d.Document == document);

        /// <summary>Devuelve todos los médicos.</summary>
        public List<Doctor> GetAll() => _doctors.ToList();

        /// <summary>Filtra médicos por especialidad (contiene, case-insensitive).</summary>
        public List<Doctor> GetBySpecialty(string specialty)
            => _doctors.Where(d => d.Specialty.ToLower().Contains(specialty.ToLower())).ToList();
    }
}
