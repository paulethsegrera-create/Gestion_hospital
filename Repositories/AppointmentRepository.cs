using System;
using System.Collections.Generic;
using System.Linq;
using Gestion_Hospital.Interfaces;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly List<Appointment> _appointments = new();
        private int _nextId = 1;

        public void Add(Appointment appointment)
        {
            appointment.Id = _nextId++;
            _appointments.Add(appointment);
        }

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

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null) _appointments.Remove(existing);
        }

        public Appointment? GetById(int id) => _appointments.FirstOrDefault(a => a.Id == id);

        public List<Appointment> GetByPatient(int patientId)
            => _appointments.Where(a => a.PatientId == patientId).OrderBy(a => a.Start).ToList();

        public List<Appointment> GetByDoctor(int doctorId)
            => _appointments.Where(a => a.DoctorId == doctorId).OrderBy(a => a.Start).ToList();

        public List<Appointment> GetAll() => _appointments.ToList();

        // Conflicto si se solapa: a.Start < end && start < a.End
        public bool ExistsConflictForDoctor(int doctorId, DateTime start, DateTime end)
            => _appointments.Any(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Scheduled
                                     && a.Start < end && start < a.End);

        public bool ExistsConflictForPatient(int patientId, DateTime start, DateTime end)
            => _appointments.Any(a => a.PatientId == patientId && a.Status == AppointmentStatus.Scheduled
                                     && a.Start < end && start < a.End);
    }
}
