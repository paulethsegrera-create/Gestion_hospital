using System.Collections.Generic; // Importa la colección genérica List<T> y otras colecciones
using Gestion_Hospital.Models; // Importa el espacio de nombres donde se encuentra la clase Doctor

namespace Gestion_Hospital.Interfaces // Define el namespace para organizar el código de interfaces del hospital
{
    /// <summary>
    /// Interface for doctor repository operations (CRUD and queries).
    /// </summary>
    public interface IDoctorRepository
    {
        /// <summary>Adds a new doctor.</summary>
        void Add(Doctor doctor);
        /// <summary>Updates an existing doctor.</summary>
        void Update(Doctor doctor);
        /// <summary>Deletes a doctor by Id.</summary>
        void Delete(int id);
        /// <summary>Gets a doctor by Id.</summary>
        Doctor? GetById(int id);
        /// <summary>Finds a doctor by document.</summary>
        Doctor? GetByDocument(string document);
        /// <summary>Returns all doctors.</summary>
        List<Doctor> GetAll();
        /// <summary>Returns doctors filtered by specialty.</summary>
        List<Doctor> GetBySpecialty(string specialty);
    }
}
