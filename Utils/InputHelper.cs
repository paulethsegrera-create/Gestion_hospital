using System;
using Gestion_Hospital.Utils;

namespace Gestion_Hospital.Utils
{
    public static class InputHelper
    {
    // Requests a string with prompt; if allowEmpty=false, repeats until it is not empty
        public static string PromptString(string prompt, bool allowEmpty = false)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine() ?? "";
                if (!allowEmpty && string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Entrada vacía. Inténtalo de nuevo.");
                    continue;
                }
                return input;
            }
        }

    // Requests and validates name with retries
        public static string PromptName(string prompt)
        {
            while (true)
            {
                var name = PromptString(prompt, false).Trim();
                if (!Validator.IsValidName(name))
                {
                    Console.WriteLine("Nombre inválido. Debe tener entre 2 y 100 caracteres y contener solo letras y espacios.");
                    continue;
                }
                return name;
            }
        }

    // Requests and validates document with retries
        public static string PromptDocument(string prompt)
        {
            while (true)
            {
                var doc = PromptString(prompt, false).Trim();
                if (!Validator.IsValidDocument(doc))
                {
                    Console.WriteLine("Documento inválido. Debe tener entre 4 y 20 caracteres alfanuméricos.");
                    continue;
                }
                return doc;
            }
        }

    // Requests and validates age with retries
        public static int PromptAge(string prompt)
        {
            while (true)
            {
                var s = PromptString(prompt, false);
                if (!int.TryParse(s, out int age) || !Validator.IsValidAge(age))
                {
                    Console.WriteLine("Edad inválida. Ingresa un número entre 0 y 120.");
                    continue;
                }
                return age;
            }
        }

    // Requests and validates email with retries
        public static string PromptEmail(string prompt)
        {
            while (true)
            {
                var email = PromptString(prompt, false).Trim();
                var normalized = Validator.NormalizeEmail(email);
                if (!Validator.IsValidEmail(normalized))
                {
                    Console.WriteLine("Correo inválido. Ingresa un correo en formato válido (ej: usuario@dominio.com).\nDeseas reintentar? (s/n)");
                    var r = Console.ReadLine() ?? "";
                    if (r.Trim().ToLowerInvariant().StartsWith("s")) continue;
                    return normalized; // si el usuario no desea reintentar, devolvemos lo ingresado normalizado
                }
                return normalized;
            }
        }

    // Requests and validates phone number with retries; if truncated, notifies and accepts if the result is valid
    // Returns a tuple (normalizedPhone, truncated)
        public static (string phone, bool truncated) PromptPhone(string prompt)
        {
            while (true)
            {
                var phoneInput = PromptString(prompt, false);
                var normalized = Validator.NormalizePhone(phoneInput, out bool truncated);
                if (!Validator.IsValidPhone(normalized))
                {
                    Console.WriteLine("Teléfono inválido. Debe contener entre 7 y 10 dígitos. Deseas reintentar? (s/n)");
                    var r = Console.ReadLine() ?? "";
                    if (r.Trim().ToLowerInvariant().StartsWith("s")) continue;
                    return (normalized, truncated); // devuelve lo que se tenga (posiblemente vacío) si el usuario no reintenta
                }
                if (truncated) Console.WriteLine("⚠️ El teléfono fue truncado a los 10 dígitos finales.");
                return (normalized, truncated);
            }
        }
    }
}
