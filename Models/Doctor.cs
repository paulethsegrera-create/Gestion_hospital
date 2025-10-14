using Gestion_Hospital.Interfaces;

namespace Gestion_Hospital.Models
{
    public class Doctor: Person, IRegistrable
    {
        public string Specialty { get; set; } = "";
    }
}