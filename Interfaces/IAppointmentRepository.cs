using System;
using System.Collections.Generic;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Interfaces
{
    public interface IAppointmentRepository
    {
        void Add(Appointment appointment);
        void Update(Appointment appointment);
        Appointment? GetById(int id);
        List<Appointment> GetByPatient(int patientId);
        List<Appointment> GetByDoctor(int doctorId);
        List<Appointment> GetAll();
        bool ExistsConflictForDoctor(int doctorId, DateTime start, DateTime end);
        bool ExistsConflictForPatient(int patientId, DateTime start, DateTime end);
    }
}
