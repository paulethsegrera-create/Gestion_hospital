using System.Collections.Generic; // Importa colecciones genéricas como List<T>
using Gestion_Hospital.Models; // Importa el espacio de nombres donde se encuentra la clase EmailHistory

namespace Gestion_Hospital.Interfaces // Define el namespace para organizar las interfaces del hospital
{
    /// <summary>
    /// Interface for the email sending service.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email.
        /// Receives recipient, subject, and message body.
        /// Returns an EmailHistory object representing the sent email record.
        /// </summary>
        EmailHistory SendEmail(string to, string subject, string body);

        /// <summary>
        /// Gets the history of sent emails.
        /// Returns a list of EmailHistory objects.
        /// </summary>
        List<EmailHistory> GetHistory();
    }
}
