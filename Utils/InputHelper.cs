using System;
using Gestion_Hospital.Utils;

namespace Gestion_Hospital.Utils
{
    public static class InputHelper
    {
        // Pide un string con prompt; si allowEmpty=false, repite hasta que no esté vacío
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

        // Pide y valida nombre con reintentos
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

        // Pide y valida documento con reintentos
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

        // Pide y valida edad con reintentos
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

        // Pide y valida email con reintentos
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

        // Pide y valida teléfono con reintentos; en caso de truncado avisa y acepta si el resultado es válido
        // Devuelve una tupla (telefonoNormalizado, truncado)
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
