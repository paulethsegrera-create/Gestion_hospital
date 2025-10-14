using System;

namespace Gestion_Hospital.Models
{
    /// <summary>
    /// Estados posibles de una cita.
    /// </summary>
    public enum AppointmentStatus { Scheduled, Cancelled, Attended }

    /// <summary>
    /// Representa una cita médica entre un paciente y un médico.
    /// </summary>
    public class Appointment
    {
        /// <summary>Identificador de la cita.</summary>
        public int Id { get; set; }

        /// <summary>Id del paciente asociado.</summary>
        public int PatientId { get; set; }

        /// <summary>Referencia al paciente (puede ser nula en ciertos contextos).</summary>
        public Patient? Patient { get; set; }

        /// <summary>Id del médico asociado.</summary>
        public int DoctorId { get; set; }

        /// <summary>Referencia al médico (puede ser nula en ciertos contextos).</summary>
        public Doctor? Doctor { get; set; }

        /// <summary>Fecha y hora de inicio.</summary>
        public DateTime Start { get; set; }

        /// <summary>Fecha y hora de fin.</summary>
        public DateTime End { get; set; }

        /// <summary>Estado de la cita.</summary>
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        /// <summary>Notas opcionales (por ejemplo, estado del envío de correo).</summary>
        public string? Notes { get; set; }
    }
}