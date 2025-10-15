using System.ComponentModel.DataAnnotations;

namespace HospitalSanVicente.Models
{
    public class Patient
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Document { get; set; } = string.Empty;
        
        [Required]
        public int Age { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navegación
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
