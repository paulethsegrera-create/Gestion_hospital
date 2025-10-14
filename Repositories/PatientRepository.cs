using System.Collections.Generic;
using System.Linq;
using Gestion_Hospital.Interfaces;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Repositories
{
    /// <summary>
    /// Repositorio en memoria para <see cref="Patient"/>. Implementa operaciones básicas CRUD.
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        private readonly List<Patient> _patients = new();
        private int _nextId = 1;

        /// <summary>Agrega un nuevo paciente (se asigna Id automáticamente).</summary>
        public void Add(Patient patient)
        {
            patient.Id = _nextId++;
            _patients.Add(patient);
        }

        /// <summary>Actualiza los datos del paciente existente.</summary>
        public void Update(Patient patient)
        {
            var existing = GetById(patient.Id);
            if (existing == null) return;
            existing.Name = patient.Name;
            existing.Document = patient.Document;
            existing.PhoneNumber = patient.PhoneNumber;
            existing.Email = patient.Email;
            existing.Age = patient.Age;
        }

        /// <summary>Elimina un paciente por Id.</summary>
        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null) _patients.Remove(existing);
        }

        /// <summary>Obtiene un paciente por Id.</summary>
        public Patient? GetById(int id) => _patients.FirstOrDefault(p => p.Id == id);

        /// <summary>Busca un paciente por documento.</summary>
        public Patient? GetByDocument(string document) => _patients.FirstOrDefault(p => p.Document == document);

        /// <summary>Devuelve todos los pacientes.</summary>
        public List<Patient> GetAll() => _patients.ToList();
    }
}
