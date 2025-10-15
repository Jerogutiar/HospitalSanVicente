using Microsoft.EntityFrameworkCore;
using HospitalSanVicente.Data;
using HospitalSanVicente.Models;
using HospitalSanVicente.Utils;

namespace HospitalSanVicente.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly HospitalDbContext _context;

        public DoctorService(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.Appointments)
                .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor?> GetDoctorByDocumentAsync(string document)
        {
            return await _context.Doctors
                .Include(d => d.Appointments)
                .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(d => d.Document == document);
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.Doctors
                .Include(d => d.Appointments)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<List<Doctor>> GetDoctorsBySpecialtyAsync(string specialty)
        {
            return await _context.Doctors
                .Where(d => d.Specialty.ToLower().Contains(specialty.ToLower()))
                .Include(d => d.Appointments)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<List<string>> GetAllSpecialtiesAsync()
        {
            return await _context.Doctors
                .Select(d => d.Specialty)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();
        }

        public async Task<List<Doctor>> SearchDoctorsAsync(string searchTerm)
        {
            return await _context.Doctors
                .Where(d => d.Name.Contains(searchTerm) || 
                           d.Document.Contains(searchTerm) ||
                           d.Specialty.Contains(searchTerm) ||
                           d.Email.Contains(searchTerm))
                .Include(d => d.Appointments)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<Doctor> CreateDoctorAsync(Doctor doctor)
        {
            // Validar que no exista un médico con el mismo documento
            var existingDoctor = await GetDoctorByDocumentAsync(doctor.Document);
            if (existingDoctor != null)
            {
                throw new InvalidOperationException($"Ya existe un médico con el documento {doctor.Document}");
            }

            // Validar que no exista un médico con la misma combinación de nombre y especialidad
            var existingDoctorByNameAndSpecialty = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Name.ToLower() == doctor.Name.ToLower() && 
                                        d.Specialty.ToLower() == doctor.Specialty.ToLower());
            
            if (existingDoctorByNameAndSpecialty != null)
            {
                throw new InvalidOperationException(
                    $"Ya existe un médico con el nombre '{doctor.Name}' y especialidad '{doctor.Specialty}'");
            }

            // Validar datos requeridos
            ValidateDoctorData(doctor);

            doctor.CreatedAt = DateTime.Now;
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<Doctor> UpdateDoctorAsync(int id, Doctor updatedDoctor)
        {
            var existingDoctor = await GetDoctorByIdAsync(id);
            if (existingDoctor == null)
            {
                throw new ArgumentException($"No se encontró un médico con ID {id}");
            }

            // Validar que el documento no esté duplicado (si se cambió)
            if (existingDoctor.Document != updatedDoctor.Document)
            {
                var doctorWithSameDocument = await GetDoctorByDocumentAsync(updatedDoctor.Document);
                if (doctorWithSameDocument != null && doctorWithSameDocument.Id != id)
                {
                    throw new InvalidOperationException($"Ya existe un médico con el documento {updatedDoctor.Document}");
                }
            }

            // Validar que no exista un médico con la misma combinación de nombre y especialidad (si se cambió)
            if (existingDoctor.Name.ToLower() != updatedDoctor.Name.ToLower() || 
                existingDoctor.Specialty.ToLower() != updatedDoctor.Specialty.ToLower())
            {
                var doctorWithSameNameAndSpecialty = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.Name.ToLower() == updatedDoctor.Name.ToLower() && 
                                            d.Specialty.ToLower() == updatedDoctor.Specialty.ToLower() &&
                                            d.Id != id);
                
                if (doctorWithSameNameAndSpecialty != null)
                {
                    throw new InvalidOperationException(
                        $"Ya existe un médico con el nombre '{updatedDoctor.Name}' y especialidad '{updatedDoctor.Specialty}'");
                }
            }

            // Validar datos requeridos
            ValidateDoctorData(updatedDoctor);

            // Actualizar propiedades
            existingDoctor.Name = updatedDoctor.Name;
            existingDoctor.Document = updatedDoctor.Document;
            existingDoctor.Specialty = updatedDoctor.Specialty;
            existingDoctor.Phone = updatedDoctor.Phone;
            existingDoctor.Email = updatedDoctor.Email;

            await _context.SaveChangesAsync();
            return existingDoctor;
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return false;
            }

            // Verificar si el médico tiene citas futuras
            var futureAppointments = doctor.Appointments
                .Where(a => a.AppointmentDate >= DateTime.Today && 
                           a.Status == AppointmentStatus.Scheduled)
                .ToList();

            if (futureAppointments.Any())
            {
                throw new InvalidOperationException(
                    $"No se puede eliminar el médico porque tiene {futureAppointments.Count} cita(s) programada(s)");
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetDoctorAppointmentsByStatusAsync(int doctorId, AppointmentStatus status)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId && a.Status == status)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetDoctorAppointmentsByDateAsync(int doctorId, DateTime date)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId && 
                           a.AppointmentDate.Date == date.Date &&
                           a.Status == AppointmentStatus.Scheduled)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();
        }

        private void ValidateDoctorData(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor), "El médico no puede ser nulo");

            // Validar nombre
            if (!ValidationHelper.IsValidName(doctor.Name))
                throw new ArgumentException("Nombre inválido");

            // Validar documento
            if (!ValidationHelper.IsValidDocument(doctor.Document))
                throw new ArgumentException("Documento inválido");

            // Validar especialidad
            if (!ValidationHelper.IsValidSpecialty(doctor.Specialty))
                throw new ArgumentException("Especialidad inválida");

            // Validar teléfono
            if (!ValidationHelper.IsValidPhone(doctor.Phone))
                throw new ArgumentException("Teléfono inválido");

            // Validar email
            if (!ValidationHelper.IsValidEmail(doctor.Email))
                throw new ArgumentException("Correo electrónico inválido");

        }

        public async Task<bool> DoctorExistsByDocumentAsync(string document, int? excludeId = null)
        {
            var query = _context.Doctors.Where(d => d.Document == document);
            
            if (excludeId.HasValue)
            {
                query = query.Where(d => d.Id != excludeId.Value);
            }
            
            return await query.AnyAsync();
        }

        public async Task<bool> DoctorExistsByNameAndSpecialtyAsync(string name, string specialty, int? excludeId = null)
        {
            var query = _context.Doctors.Where(d => d.Name == name && d.Specialty == specialty);
            
            if (excludeId.HasValue)
            {
                query = query.Where(d => d.Id != excludeId.Value);
            }
            
            return await query.AnyAsync();
        }

    }
}
