using Microsoft.EntityFrameworkCore;
using HospitalSanVicente.Data;
using HospitalSanVicente.Models;
using HospitalSanVicente.Utils;

namespace HospitalSanVicente.Services
{
    public class PatientService : IPatientService
    {
        private readonly HospitalDbContext _context;

        public PatientService(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<Patient?> GetPatientByIdAsync(int id)
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Patient?> GetPatientByDocumentAsync(string document)
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
                .FirstOrDefaultAsync(p => p.Document == document);
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<List<Patient>> SearchPatientsAsync(string searchTerm)
        {
            return await _context.Patients
                .Where(p => p.Name.Contains(searchTerm) || 
                           p.Document.Contains(searchTerm) ||
                           p.Email.Contains(searchTerm))
                .Include(p => p.Appointments)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Patient> CreatePatientAsync(Patient patient)
        {
            // Validaciones básicas
            if (patient == null)
                throw new ArgumentNullException(nameof(patient), "El paciente no puede ser nulo");

            // Validar datos requeridos
            ValidatePatientData(patient);

            // Limpiar espacios en blanco
            patient.Name = patient.Name.Trim();
            patient.Document = patient.Document.Trim();
            patient.Phone = patient.Phone.Trim();
            patient.Email = patient.Email.Trim();

            patient.CreatedAt = DateTime.Now;
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient> UpdatePatientAsync(int id, Patient updatedPatient)
        {
            // Validaciones de entrada
            if (!ValidationHelper.IsValidId(id))
                throw new ArgumentException("ID de paciente inválido");

            if (updatedPatient == null)
                throw new ArgumentNullException(nameof(updatedPatient), "El paciente no puede ser nulo");

            var existingPatient = await GetPatientByIdAsync(id);
            if (existingPatient == null)
            {
                throw new ArgumentException($"No se encontró un paciente con ID {id}");
            }

            // Validar datos requeridos
            ValidatePatientData(updatedPatient);

            // Validar que el documento no esté duplicado (si se cambió)
            if (existingPatient.Document != updatedPatient.Document)
            {
                var patientWithSameDocument = await GetPatientByDocumentAsync(updatedPatient.Document);
                if (patientWithSameDocument != null && patientWithSameDocument.Id != id)
                {
                    throw new InvalidOperationException($"Ya existe un paciente con el documento {updatedPatient.Document}");
                }
            }

            // Validar que el email no esté duplicado (si se cambió)
            if (existingPatient.Email.ToLower() != updatedPatient.Email.ToLower())
            {
                var patientWithSameEmail = await _context.Patients
                    .FirstOrDefaultAsync(p => p.Email.ToLower() == updatedPatient.Email.ToLower() && p.Id != id);
                if (patientWithSameEmail != null)
                {
                    throw new InvalidOperationException($"Ya existe un paciente con el email {updatedPatient.Email}");
                }
            }

            // Limpiar espacios en blanco
            updatedPatient.Name = updatedPatient.Name.Trim();
            updatedPatient.Document = updatedPatient.Document.Trim();
            updatedPatient.Phone = updatedPatient.Phone.Trim();
            updatedPatient.Email = updatedPatient.Email.Trim();

            // Actualizar propiedades
            existingPatient.Name = updatedPatient.Name;
            existingPatient.Document = updatedPatient.Document;
            existingPatient.Age = updatedPatient.Age;
            existingPatient.Phone = updatedPatient.Phone;
            existingPatient.Email = updatedPatient.Email;

            await _context.SaveChangesAsync();
            return existingPatient;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            // Validaciones de entrada
            if (!ValidationHelper.IsValidId(id))
                throw new ArgumentException("ID de paciente inválido");

            var patient = await GetPatientByIdAsync(id);
            if (patient == null)
            {
                return false;
            }

            // Verificar si el paciente tiene citas futuras
            var futureAppointments = patient.Appointments
                .Where(a => a.AppointmentDate >= DateTime.Today && 
                           a.Status == AppointmentStatus.Scheduled)
                .ToList();

            if (futureAppointments.Any())
            {
                throw new InvalidOperationException(
                    $"No se puede eliminar el paciente porque tiene {futureAppointments.Count} cita(s) programada(s)");
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Appointment>> GetPatientAppointmentsAsync(int patientId)
        {
            // Validaciones de entrada
            if (!ValidationHelper.IsValidId(patientId))
                throw new ArgumentException("ID de paciente inválido");

            return await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetPatientAppointmentsByStatusAsync(int patientId, AppointmentStatus status)
        {
            // Validaciones de entrada
            if (!ValidationHelper.IsValidId(patientId))
                throw new ArgumentException("ID de paciente inválido");

            if (!Enum.IsDefined(typeof(AppointmentStatus), status))
                throw new ArgumentException("Estado de cita inválido");

            return await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId && a.Status == status)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.AppointmentTime)
                .ToListAsync();
        }

        private void ValidatePatientData(Patient patient)
        {
            if (patient == null)
                throw new ArgumentNullException(nameof(patient), "El paciente no puede ser nulo");

            // Validar nombre
            if (!ValidationHelper.IsValidName(patient.Name))
                throw new ArgumentException("Nombre inválido");

            // Validar documento
            if (!ValidationHelper.IsValidDocument(patient.Document))
                throw new ArgumentException("Documento inválido");

            // Validar edad
            if (!ValidationHelper.IsValidAge(patient.Age))
                throw new ArgumentException("Edad inválida");

            // Validar teléfono
            if (!ValidationHelper.IsValidPhone(patient.Phone))
                throw new ArgumentException("Teléfono inválido");

            // Validar email
            if (!ValidationHelper.IsValidEmail(patient.Email))
                throw new ArgumentException("Correo electrónico inválido");

        }

        public async Task<bool> PatientExistsByDocumentAsync(string document, int? excludeId = null)
        {
            var query = _context.Patients.Where(p => p.Document == document);
            
            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }
            
            return await query.AnyAsync();
        }

    }
}
