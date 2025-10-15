using Microsoft.EntityFrameworkCore;
using HospitalSanVicente.Models;

namespace HospitalSanVicente.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de índices únicos (temporalmente deshabilitados para pruebas)
            // modelBuilder.Entity<Patient>()
            //     .HasIndex(p => p.Document)
            //     .IsUnique();

            // modelBuilder.Entity<Doctor>()
            //     .HasIndex(d => d.Document)
            //     .IsUnique();

            // Configuración de relaciones
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmailLog>()
                .HasOne(e => e.Appointment)
                .WithMany()
                .HasForeignKey(e => e.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de validaciones adicionales
            modelBuilder.Entity<Patient>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Patient>()
                .Property(p => p.Document)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Patient>()
                .Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Document)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Specialty)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Email)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
