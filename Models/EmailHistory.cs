namespace Gestion_Hospital.Models // Define el namespace donde se encuentran los modelos de datos del hospital
{
    /// <summary>
    /// Registro que representa un envío de correo (simulado) dentro del sistema.
    /// Se usa para auditar intentos de envío y errores asociados.
    /// </summary>
    public class EmailHistory
    {
        /// <summary>Identificador del registro.</summary>
        public int Id { get; set; }

        /// <summary>Destinatario (To) del correo.</summary>
        public string To { get; set; } = "";

        /// <summary>Asunto del correo.</summary>
        public string Subject { get; set; } = "";

        /// <summary>Cuerpo del correo.</summary>
        public string Body { get; set; } = "";

        /// <summary>Indica si el correo fue enviado correctamente.</summary>
        public bool Sent { get; set; }

        /// <summary>Fecha y hora del intento de envío.</summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>Mensaje de error si el envío falló.</summary>
        public string? ErrorMessage { get; set; }
    }
}
