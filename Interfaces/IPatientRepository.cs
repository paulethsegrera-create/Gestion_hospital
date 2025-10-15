using System.Collections.Generic;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Interfaces
{
    /// <summary>
    /// Interface for patient repository operations (CRUD and queries).
    /// </summary>
    public interface IPatientRepository
    {
        /// <summary>Adds a new patient.</summary>
        void Add(Patient patient);
        /// <summary>Updates an existing patient.</summary>
        void Update(Patient patient);
        /// <summary>Deletes a patient by Id.</summary>
        void Delete(int id);
        /// <summary>Gets a patient by Id.</summary>
        Patient? GetById(int id);
        /// <summary>Finds a patient by document.</summary>
        Patient? GetByDocument(string document);
        /// <summary>Returns all patients.</summary>
        List<Patient> GetAll();
    }
}
