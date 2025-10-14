namespace Gestion_Hospital.Models
{
    public abstract class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Document { get; set; } = "";
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; }  = "";
    }
}