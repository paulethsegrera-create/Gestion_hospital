using System.Collections.Generic;
using System.Linq;
using Gestion_Hospital.Interfaces;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly List<Patient> _patients = new();
        private int _nextId = 1;

        public void Add(Patient patient)
        {
            patient.Id = _nextId++;
            _patients.Add(patient);
        }

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

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null) _patients.Remove(existing);
        }

        public Patient? GetById(int id) => _patients.FirstOrDefault(p => p.Id == id);
        public Patient? GetByDocument(string document) => _patients.FirstOrDefault(p => p.Document == document);
        public List<Patient> GetAll() => _patients.ToList();
    }
}
