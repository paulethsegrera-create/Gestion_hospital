namespace Gestion_Hospital.Models
{
    public class Appoiment
    {
        public enum AppointmentStatus { Scheduled, Cancelled, Attended }

        public class Appointment
        {
            public int Id { get; set; }
            public int PatientId { get; set; }
            public Patient? Patient { get; set; }
            public int DoctorId { get; set; }
            public Doctor? Doctor { get; set; }
            public DateTime Start { get; set; }    // fecha y hora inicio
            public DateTime End { get; set; }      // fecha y hora fin
            public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
            public string? Notes { get; set; }
        }
    }
}