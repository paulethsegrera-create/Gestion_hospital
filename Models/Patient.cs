using Gestion_Hospital.Interfaces;

namespace Gestion_Hospital.Models
{
    public class Patient : Person, IRegistrable
    {
        public int Age { get; set; }
    }
}
