using System.ComponentModel.DataAnnotations;

namespace HospitalSanVicente.Models
{
    public enum EmailStatus
    {
        Sent = 1,
        NotSent = 2,
        Failed = 3
    }

    public class EmailLog
    {
        public int Id { get; set; }
        
        [Required]
        public int AppointmentId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string RecipientEmail { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Body { get; set; } = string.Empty;
        
        [Required]
        public EmailStatus Status { get; set; }
        
        public string? ErrorMessage { get; set; }
        
        public DateTime SentAt { get; set; } = DateTime.Now;
        
        // Navegación
        public virtual Appointment Appointment { get; set; } = null!;
    }
}
