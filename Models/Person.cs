namespace Gestion_Hospital.Models
{
    /// <summary>
    /// Clase base para personas (pacientes, médicos, etc.).
    /// Contiene datos comunes como nombre, documento y teléfono y
    /// métodos para obtener versiones enmascaradas de datos sensibles.
    /// </summary>
    public abstract class Person 
    {
        /// <summary>Identificador único.</summary>
        public int Id { get; set; }

        /// <summary>Nombre de la persona.</summary>
        public string Name { get; set; } = "";

        /// <summary>Apellido de la persona.</summary>
        public string Surname { get; set; } = "";

        private string _document = "";
        private string _phoneNumber = "";

        /// <summary>Dirección física.</summary>
        public string Address { get; set; } = "";

        /// <summary>Correo electrónico.</summary>
        public string Email { get; set; }  = "";

        /// <summary>Documento de identidad (encapsulado internamente).</summary>
        public string Document
        {
            get => _document;
            set => _document = value ?? "";
        }

        /// <summary>Número de teléfono (encapsulado internamente).</summary>
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value ?? "";
        }

        /// <summary>
        /// Retorna el documento parcialmente enmascarado, mostrando solo los últimos 4 caracteres si aplica.
        /// </summary>
        public string MaskDocument()
        {
            if (string.IsNullOrWhiteSpace(Document)) return string.Empty;
            var d = Document.Trim();
            if (d.Length <= 4) return new string('*', d.Length);
            return new string('*', d.Length - 4) + d.Substring(d.Length - 4);
        }

        /// <summary>
        /// Retorna el teléfono parcialmente enmascarado, mostrando los últimos 4 dígitos.
        /// </summary>
        public string MaskPhone()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber)) return string.Empty;
            var p = PhoneNumber.Trim();
            if (p.Length <= 4) return new string('*', p.Length);
            return new string('*', p.Length - 4) + p.Substring(p.Length - 4);
        }
    }
}