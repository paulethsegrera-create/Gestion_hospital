namespace Gestion_Hospital.Models
{
    public abstract class Person 
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        private string _document = "";
        private string _phoneNumber = "";
        public string Address { get; set; } = "";
        public string Email { get; set; }  = "";

        public string Document
        {
            get => _document;
            set => _document = value ?? "";
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value ?? "";
        }

        // Retorna el documento parcialmente enmascarado, mostrando solo los últimos 4 caracteres si aplica
        public string MaskDocument()
        {
            if (string.IsNullOrWhiteSpace(Document)) return string.Empty;
            var d = Document.Trim();
            if (d.Length <= 4) return new string('*', d.Length);
            return new string('*', d.Length - 4) + d.Substring(d.Length - 4);
        }

        // Retorna el teléfono parcialmente enmascarado, mostrando los últimos 4 dígitos
        public string MaskPhone()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber)) return string.Empty;
            var p = PhoneNumber.Trim();
            if (p.Length <= 4) return new string('*', p.Length);
            return new string('*', p.Length - 4) + p.Substring(p.Length - 4);
        }
    }
}