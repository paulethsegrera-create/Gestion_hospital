using System;
using System.Collections.Generic;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Interfaces
{
    /// <summary>
    /// Interface that defines the contract for the appointment repository (Appointment).
    /// Responsible for CRUD operations and schedule conflict validations.
    /// </summary>
    public interface IAppointmentRepository
    {
        /// <summary>Adds a new appointment to the repository.</summary>
        void Add(Appointment appointment);

        /// <summary>Updates the data of an existing appointment.</summary>
        void Update(Appointment appointment);

        /// <summary>Deletes an appointment from the repository by its Id.</summary>
        void Delete(int id);

        /// <summary>Gets a specific appointment by its Id.</summary>
        Appointment? GetById(int id);

        /// <summary>Gets all appointments associated with a specific patient.</summary>
        List<Appointment> GetByPatient(int patientId);

        /// <summary>Gets all appointments associated with a specific doctor.</summary>
        List<Appointment> GetByDoctor(int doctorId);

        /// <summary>Returns a complete list of all registered appointments.</summary>
        List<Appointment> GetAll();

        /// <summary>Checks if there is a conflicting appointment for the same doctor in the specified time range (prevents overlapping).</summary>
        bool ExistsConflictForDoctor(int doctorId, DateTime start, DateTime end);

        /// <summary>Checks if there is a conflicting appointment for the same patient in the specified time range (prevents double booking).</summary>
        bool ExistsConflictForPatient(int patientId, DateTime start, DateTime end);
    }
}
