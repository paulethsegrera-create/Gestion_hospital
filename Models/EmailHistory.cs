namespace Gestion_Hospital.Models // Define el namespace donde se encuentran los modelos de datos del hospital
{
    /// <summary>
    /// Record representing an email sending (simulated) within the system.
    /// Used to audit sending attempts and associated errors.
    /// </summary>
    public class EmailHistory
    {
    /// <summary>Record identifier.</summary>
        public int Id { get; set; }

    /// <summary>Email recipient (To).</summary>
        public string To { get; set; } = "";

    /// <summary>Email subject.</summary>
        public string Subject { get; set; } = "";

    /// <summary>Email body.</summary>
        public string Body { get; set; } = "";

    /// <summary>Indicates if the email was sent successfully.</summary>
        public bool Sent { get; set; }

    /// <summary>Date and time of the sending attempt.</summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

    /// <summary>Error message if sending failed.</summary>
        public string? ErrorMessage { get; set; }
    }
}
