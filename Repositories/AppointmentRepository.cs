using System;
using System.Collections.Generic;
using System.Linq;
using Gestion_Hospital.Interfaces;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Repositories
{
    /// <summary>
    /// In-memory repository for <see cref="Appointment"/>. Implements CRUD and specific queries.
    /// </summary>
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly List<Appointment> _appointments = new();
        private int _nextId = 1;

    /// <summary>Adds a new appointment and assigns Id automatically.</summary>
        public void Add(Appointment appointment)
        {
            appointment.Id = _nextId++;
            _appointments.Add(appointment);
        }

    /// <summary>Updates an existing appointment.</summary>
        public void Update(Appointment appointment)
        {
            var existing = GetById(appointment.Id);
            if (existing == null) return;
            existing.PatientId = appointment.PatientId;
            existing.DoctorId = appointment.DoctorId;
            existing.Start = appointment.Start;
            existing.End = appointment.End;
            existing.Status = appointment.Status;
            existing.Notes = appointment.Notes;
        }

    /// <summary>Deletes an appointment by Id.</summary>
        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null) _appointments.Remove(existing);
        }

    /// <summary>Gets an appointment by Id.</summary>
        public Appointment? GetById(int id) => _appointments.FirstOrDefault(a => a.Id == id);

    /// <summary>Lists a patient's appointments ordered by start time.</summary>
        public List<Appointment> GetByPatient(int patientId)
            => _appointments.Where(a => a.PatientId == patientId).OrderBy(a => a.Start).ToList();

    /// <summary>Lists a doctor's appointments ordered by start time.</summary>
        public List<Appointment> GetByDoctor(int doctorId)
            => _appointments.Where(a => a.DoctorId == doctorId).OrderBy(a => a.Start).ToList();

    /// <summary>Returns all appointments.</summary>
        public List<Appointment> GetAll() => _appointments.ToList();

        // Conflicto si se solapa: a.Start < end && start < a.End
    /// <summary>Checks if there is a schedule conflict for a doctor.</summary>
        public bool ExistsConflictForDoctor(int doctorId, DateTime start, DateTime end)
            => _appointments.Any(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Scheduled
                                     && a.Start < end && start < a.End);

    /// <summary>Checks if there is a schedule conflict for a patient.</summary>
        public bool ExistsConflictForPatient(int patientId, DateTime start, DateTime end)
            => _appointments.Any(a => a.PatientId == patientId && a.Status == AppointmentStatus.Scheduled
                                     && a.Start < end && start < a.End);
    }
}
