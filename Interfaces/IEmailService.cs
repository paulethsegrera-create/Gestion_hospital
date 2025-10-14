using System.Collections.Generic; // Importa colecciones genéricas como List<T>
using Gestion_Hospital.Models; // Importa el espacio de nombres donde se encuentra la clase EmailHistory

namespace Gestion_Hospital.Interfaces // Define el namespace para organizar las interfaces del hospital
{
    // Define una interfaz para el servicio de envío de correos electrónicos
    public interface IEmailService
    {
        // Método para enviar un correo electrónico
        // Recibe destinatario, asunto y cuerpo del mensaje
        // Devuelve un objeto EmailHistory que representa el registro del correo enviado
        EmailHistory SendEmail(string to, string subject, string body);

        // Método para obtener el historial de correos enviados
        // Devuelve una lista de objetos EmailHistory
        List<EmailHistory> GetHistory();
    }
}
