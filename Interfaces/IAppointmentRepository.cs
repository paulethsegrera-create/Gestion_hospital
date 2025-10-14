using System;
using System.Collections.Generic;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Interfaces
{
    // Interfaz que define el contrato para el repositorio de citas médicas (Appointment)
    // Se encarga de definir las operaciones CRUD y las validaciones de conflictos de horario
    public interface IAppointmentRepository
    {
        // Agrega una nueva cita al repositorio
        void Add(Appointment appointment);

        // Actualiza los datos de una cita existente
        void Update(Appointment appointment);

        // Elimina una cita del repositorio según su Id
        void Delete(int id);

        // Obtiene una cita específica por su Id
        Appointment? GetById(int id);

        // Obtiene todas las citas asociadas a un paciente específico
        List<Appointment> GetByPatient(int patientId);

        // Obtiene todas las citas asociadas a un médico específico
        List<Appointment> GetByDoctor(int doctorId);

        // Devuelve una lista completa con todas las citas registradas
        List<Appointment> GetAll();

        // Verifica si ya existe una cita en conflicto para el mismo médico
        // en el rango de tiempo especificado (evita citas superpuestas)
        bool ExistsConflictForDoctor(int doctorId, DateTime start, DateTime end);

        // Verifica si ya existe una cita en conflicto para el mismo paciente
        // en el rango de tiempo especificado (evita doble agendamiento)
        bool ExistsConflictForPatient(int patientId, DateTime start, DateTime end);
    }
}
