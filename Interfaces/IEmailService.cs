using System.Collections.Generic;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Interfaces
{
    public interface IEmailService
    {
        EmailHistory SendEmail(string to, string subject, string body);
        List<EmailHistory> GetHistory();
    }
}
