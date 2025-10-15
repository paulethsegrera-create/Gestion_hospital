namespace Gestion_Hospital.Models
{
    /// <summary>
    /// Base class for people (patients, doctors, etc.).
    /// Contains common data such as name, document, and phone number,
    /// and methods to obtain masked versions of sensitive data.
    /// </summary>
    public abstract class Person 
    {
    /// <summary>Unique identifier.</summary>
        public int Id { get; set; }

    /// <summary>Person's name.</summary>
        public string Name { get; set; } = "";

    /// <summary>Person's surname.</summary>
        public string Surname { get; set; } = "";

        private string _document = "";
        private string _phoneNumber = "";

    /// <summary>Physical address.</summary>
        public string Address { get; set; } = "";

    /// <summary>Email address.</summary>
        public string Email { get; set; }  = "";

    /// <summary>Identity document (internally encapsulated).</summary>
        public string Document
        {
            get => _document;
            set => _document = value ?? "";
        }

    /// <summary>Phone number (internally encapsulated).</summary>
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value ?? "";
        }

    /// <summary>
    /// Returns the partially masked document, showing only the last 4 characters if applicable.
    /// </summary>
        public string MaskDocument()
        {
            if (string.IsNullOrWhiteSpace(Document)) return string.Empty;
            var d = Document.Trim();
            if (d.Length <= 4) return new string('*', d.Length);
            return new string('*', d.Length - 4) + d.Substring(d.Length - 4);
        }

    /// <summary>
    /// Returns the partially masked phone number, showing only the last 4 digits.
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