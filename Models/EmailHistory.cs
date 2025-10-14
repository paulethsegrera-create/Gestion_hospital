namespace Gestion_Hospital.Models
{
    public class EmailHistory
    {
         public int Id { get; set; }
        public string To { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";
        public bool Sent { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string? ErrorMessage { get; set; }
    }
}