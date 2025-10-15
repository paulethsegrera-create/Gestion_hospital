using System.Collections.Generic;
using System.Linq;
using Gestion_Hospital.Interfaces;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Repositories
{
    /// <summary>
    /// In-memory repository for <see cref="Patient"/>. Implements basic CRUD operations.
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        private readonly List<Patient> _patients = new();
        private int _nextId = 1;

    /// <summary>Adds a new patient (Id is assigned automatically).</summary>
        public void Add(Patient patient)
        {
            patient.Id = _nextId++;
            _patients.Add(patient);
        }

    /// <summary>Updates the existing patient's data.</summary>
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

    /// <summary>Deletes a patient by Id.</summary>
        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null) _patients.Remove(existing);
        }

    /// <summary>Gets a patient by Id.</summary>
        public Patient? GetById(int id) => _patients.FirstOrDefault(p => p.Id == id);

    /// <summary>Finds a patient by document.</summary>
        public Patient? GetByDocument(string document) => _patients.FirstOrDefault(p => p.Document == document);

    /// <summary>Returns all patients.</summary>
        public List<Patient> GetAll() => _patients.ToList();
    }
}
