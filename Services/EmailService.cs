using System;
using System.Collections.Generic;
using Gestion_Hospital.Interfaces;
using Gestion_Hospital.Models;
using Microsoft.Extensions.Logging;

namespace Gestion_Hospital.Services
{
    public class EmailService : IEmailService
    {
        private readonly List<EmailHistory> _histories = new();
        private int _nextId = 1;
        private readonly Microsoft.Extensions.Logging.ILogger<EmailService>? _logger;

        public EmailService(Microsoft.Extensions.Logging.ILogger<EmailService>? logger = null)
        {
            _logger = logger;
        }

        // Simulación de envío de correo. No realiza llamadas externas.
        public EmailHistory SendEmail(string to, string subject, string body)
        {
            var history = new EmailHistory
            {
                Id = _nextId++,
                To = to,
                Subject = subject,
                Body = body,
                Timestamp = DateTime.Now
            };

            try
            {
                // Simula éxito o fracaso (por ejemplo, fallo si dirección no valida)
                if (string.IsNullOrWhiteSpace(to) || !to.Contains("@"))
                    throw new Exception("Dirección de correo inválida.");

                // Simulamos envío: aquí podrías poner SMTP real si quisieras.
                _logger?.LogInformation("Enviando correo a {To} Asunto:{Subject}", to, subject);
                Console.WriteLine($"[Simulación correo] Enviando a {to}...");
                history.Sent = true;
            }
            catch (Exception ex)
            {
                history.Sent = false;
                history.ErrorMessage = ex.Message;
                _logger?.LogError(ex, "Fallo al enviar correo a {To}", to);
            }

            _histories.Add(history);
            return history;
        }

        public List<EmailHistory> GetHistory() => _histories.ToList();
    }
}
