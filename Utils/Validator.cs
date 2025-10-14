using System.Text.RegularExpressions;

namespace Gestion_Hospital.Utils
{
    public static class Validator
    {
        // Valida que el nombre no esté vacío y contenga caracteres razonables
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            name = name.Trim();
            if (name.Length < 2 || name.Length > 100) return false;
            // Permitimos letras, espacios, guiones y apóstrofes
              return Regex.IsMatch(name, @"^[\p{L} .'-]+$");
        }

        // Valida documento: aceptamos números o alfanuméricos simples, longitud entre 4 y 20
        public static bool IsValidDocument(string doc)
        {
            if (string.IsNullOrWhiteSpace(doc)) return false;
            doc = doc.Trim();
            if (doc.Length < 4 || doc.Length > 20) return false;
            return Regex.IsMatch(doc, "^[A-Za-z0-9-]+$");
        }

        public static bool IsValidAge(int age)
        {
            return age >= 0 && age <= 120;
        }

        public static string NormalizeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return string.Empty;
            return email.Trim().ToLowerInvariant();
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            // Regex simple
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            // Extraer solo dígitos
            var digits = Regex.Replace(phone, "\\D", "");
            // No permitimos más de 10 dígitos
            if (digits.Length == 0 || digits.Length > 10) return false;
            // Aceptamos números razonables (mínimo 7 dígitos, máximo 10)
            return digits.Length >= 7 && digits.Length <= 10;
        }

        /// <summary>
        /// Normaliza un string de teléfono a solo dígitos y, si tiene más de 10,
        /// lo trunca a los 10 dígitos finales (se asume que los prefijos de país van al inicio).
        /// Devuelve el teléfono normalizado y un flag indicando si hubo truncado.
        /// </summary>
        public static string NormalizePhone(string input, out bool truncated)
        {
            truncated = false;
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            var digits = Regex.Replace(input, "\\D", "");
            if (digits.Length > 10)
            {
                // Tomamos los 10 dígitos finales (por ejemplo, quitamos prefijo de país)
                digits = digits.Substring(digits.Length - 10);
                truncated = true;
            }
            return digits;
        }
    }
}
