using System;

namespace Gestion_Hospital.Models
{
    /// <summary>
    /// Possible appointment statuses.
    /// </summary>
    public enum AppointmentStatus { Scheduled, Cancelled, Attended }

    /// <summary>
    /// Represents a medical appointment between a patient and a doctor.
    /// </summary>
    public class Appointment
    {
    /// <summary>Appointment identifier.</summary>
        public int Id { get; set; }

    /// <summary>Associated patient Id.</summary>
        public int PatientId { get; set; }

    /// <summary>Reference to the patient (may be null in some contexts).</summary>
        public Patient? Patient { get; set; }

    /// <summary>Associated doctor Id.</summary>
        public int DoctorId { get; set; }

    /// <summary>Reference to the doctor (may be null in some contexts).</summary>
        public Doctor? Doctor { get; set; }

    /// <summary>Start date and time.</summary>
        public DateTime Start { get; set; }

    /// <summary>End date and time.</summary>
        public DateTime End { get; set; }

    /// <summary>Appointment status.</summary>
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    /// <summary>Optional notes (e.g., email sending status).</summary>
        public string? Notes { get; set; }
    }
}