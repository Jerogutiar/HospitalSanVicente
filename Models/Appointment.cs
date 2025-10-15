using System.ComponentModel.DataAnnotations;

namespace HospitalSanVicente.Models
{
    public enum AppointmentStatus
    {
        Scheduled = 1,
        Attended = 2,
        Cancelled = 3
    }

    public class Appointment
    {
        public int Id { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        public DateTime AppointmentDate { get; set; }
        
        [Required]
        public TimeSpan AppointmentTime { get; set; }
        
        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navegación
        public virtual Patient Patient { get; set; } = null!;
        public virtual Doctor Doctor { get; set; } = null!;
    }
}
