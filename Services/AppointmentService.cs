using Microsoft.EntityFrameworkCore;
using HospitalSanVicente.Data;
using HospitalSanVicente.Models;
using HospitalSanVicente.Utils;

namespace HospitalSanVicente.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly HospitalDbContext _context;
        private readonly IEmailService _emailService;

        public AppointmentService(HospitalDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.AppointmentDate.Date == date.Date)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.AppointmentDate.Date >= startDate.Date && 
                           a.AppointmentDate.Date <= endDate.Date)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            // Validar que el paciente existe
            var patient = await _context.Patients.FindAsync(appointment.PatientId);
            if (patient == null)
            {
                throw new ArgumentException($"No se encontró un paciente con ID {appointment.PatientId}");
            }

            // Validar que el médico existe
            var doctor = await _context.Doctors.FindAsync(appointment.DoctorId);
            if (doctor == null)
            {
                throw new ArgumentException($"No se encontró un médico con ID {appointment.DoctorId}");
            }

            // Validar datos de la cita
            ValidateAppointmentData(appointment);

            // Validar que no haya conflicto de horarios para el médico
            var doctorConflict = await _context.Appointments
                .Where(a => a.DoctorId == appointment.DoctorId &&
                           a.AppointmentDate.Date == appointment.AppointmentDate.Date &&
                           a.AppointmentTime == appointment.AppointmentTime &&
                           a.Status == AppointmentStatus.Scheduled)
                .FirstOrDefaultAsync();

            if (doctorConflict != null)
            {
                throw new InvalidOperationException(
                    $"El médico ya tiene una cita programada para el {appointment.AppointmentDate.ToString("dd/MM/yyyy")} a las {appointment.AppointmentTime.ToString(@"HH\:mm")}");
            }

            // Validar que no haya conflicto de horarios para el paciente
            var patientConflict = await _context.Appointments
                .Where(a => a.PatientId == appointment.PatientId &&
                           a.AppointmentDate.Date == appointment.AppointmentDate.Date &&
                           a.AppointmentTime == appointment.AppointmentTime &&
                           a.Status == AppointmentStatus.Scheduled)
                .FirstOrDefaultAsync();

            if (patientConflict != null)
            {
                throw new InvalidOperationException(
                    $"El paciente ya tiene una cita programada para el {appointment.AppointmentDate.ToString("dd/MM/yyyy")} a las {appointment.AppointmentTime.ToString(@"HH\:mm")}");
            }

            // Validar que la cita no sea en el pasado
            var appointmentDateTime = appointment.AppointmentDate.Date.Add(appointment.AppointmentTime);
            if (appointmentDateTime <= DateTime.Now)
            {
                throw new InvalidOperationException("No se pueden programar citas en fechas u horas pasadas");
            }

            appointment.CreatedAt = DateTime.Now;
            appointment.Status = AppointmentStatus.Scheduled;

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // Cargar las relaciones para el envío de correo
            appointment.Patient = patient;
            appointment.Doctor = doctor;

            // Enviar correo de confirmación (no debe interrumpir el flujo principal)
            try
            {
                await _emailService.SendAppointmentConfirmationAsync(appointment);
            }
            catch (Exception ex)
            {
                // Log del error pero no interrumpir el flujo principal
                // En un entorno real, aquí se podría usar un logger
                System.Diagnostics.Debug.WriteLine($"Error al enviar correo de confirmación: {ex.Message}");
            }

            return appointment;
        }

        public async Task<Appointment> UpdateAppointmentAsync(int id, Appointment updatedAppointment)
        {
            var existingAppointment = await GetAppointmentByIdAsync(id);
            if (existingAppointment == null)
            {
                throw new ArgumentException($"No se encontró una cita con ID {id}");
            }

            // Validar que el paciente existe (si se cambió)
            if (existingAppointment.PatientId != updatedAppointment.PatientId)
            {
                var patient = await _context.Patients.FindAsync(updatedAppointment.PatientId);
                if (patient == null)
                {
                    throw new ArgumentException($"No se encontró un paciente con ID {updatedAppointment.PatientId}");
                }
            }

            // Validar que el médico existe (si se cambió)
            if (existingAppointment.DoctorId != updatedAppointment.DoctorId)
            {
                var doctor = await _context.Doctors.FindAsync(updatedAppointment.DoctorId);
                if (doctor == null)
                {
                    throw new ArgumentException($"No se encontró un médico con ID {updatedAppointment.DoctorId}");
                }
            }

            // Validar datos de la cita
            ValidateAppointmentData(updatedAppointment);

            // Validar conflictos de horarios solo si se cambió la fecha, hora, médico o paciente
            if (existingAppointment.DoctorId != updatedAppointment.DoctorId ||
                existingAppointment.PatientId != updatedAppointment.PatientId ||
                existingAppointment.AppointmentDate.Date != updatedAppointment.AppointmentDate.Date ||
                existingAppointment.AppointmentTime != updatedAppointment.AppointmentTime)
            {
                // Validar que no haya conflicto de horarios para el médico
                var doctorConflict = await _context.Appointments
                    .Where(a => a.DoctorId == updatedAppointment.DoctorId &&
                               a.AppointmentDate.Date == updatedAppointment.AppointmentDate.Date &&
                               a.AppointmentTime == updatedAppointment.AppointmentTime &&
                               a.Status == AppointmentStatus.Scheduled &&
                               a.Id != id)
                    .FirstOrDefaultAsync();

                if (doctorConflict != null)
                {
                    throw new InvalidOperationException(
                        $"El médico ya tiene una cita programada para el {updatedAppointment.AppointmentDate:dd/MM/yyyy} a las {updatedAppointment.AppointmentTime:HH:mm}");
                }

                // Validar que no haya conflicto de horarios para el paciente
                var patientConflict = await _context.Appointments
                    .Where(a => a.PatientId == updatedAppointment.PatientId &&
                               a.AppointmentDate.Date == updatedAppointment.AppointmentDate.Date &&
                               a.AppointmentTime == updatedAppointment.AppointmentTime &&
                               a.Status == AppointmentStatus.Scheduled &&
                               a.Id != id)
                    .FirstOrDefaultAsync();

                if (patientConflict != null)
                {
                    throw new InvalidOperationException(
                        $"El paciente ya tiene una cita programada para el {updatedAppointment.AppointmentDate:dd/MM/yyyy} a las {updatedAppointment.AppointmentTime:HH:mm}");
                }
            }

            // Actualizar propiedades
            existingAppointment.PatientId = updatedAppointment.PatientId;
            existingAppointment.DoctorId = updatedAppointment.DoctorId;
            existingAppointment.AppointmentDate = updatedAppointment.AppointmentDate;
            existingAppointment.AppointmentTime = updatedAppointment.AppointmentTime;
            existingAppointment.Status = updatedAppointment.Status;
            existingAppointment.Notes = updatedAppointment.Notes;

            await _context.SaveChangesAsync();
            return existingAppointment;
        }

        public async Task<bool> CancelAppointmentAsync(int id)
        {
            var appointment = await GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return false;
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("La cita ya está cancelada");
            }

            if (appointment.Status == AppointmentStatus.Attended)
            {
                throw new InvalidOperationException("No se puede cancelar una cita que ya fue atendida");
            }

            appointment.Status = AppointmentStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAppointmentAsAttendedAsync(int id)
        {
            var appointment = await GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return false;
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("No se puede marcar como atendida una cita cancelada");
            }

            if (appointment.Status == AppointmentStatus.Attended)
            {
                throw new InvalidOperationException("La cita ya está marcada como atendida");
            }

            appointment.Status = AppointmentStatus.Attended;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            var appointment = await GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return false;
            }

            // Solo se pueden eliminar citas canceladas o en el pasado
            var appointmentDateTime = appointment.AppointmentDate.Date.Add(appointment.AppointmentTime);
            if (appointment.Status == AppointmentStatus.Scheduled && appointmentDateTime > DateTime.Now)
            {
                throw new InvalidOperationException("No se puede eliminar una cita programada futura. Cancele la cita primero.");
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Appointment>> GetPatientAppointmentsAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.AppointmentTime)
                .ToListAsync();
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

        public async Task<Dictionary<string, int>> GetAppointmentStatisticsAsync()
        {
            var totalAppointments = await _context.Appointments.CountAsync();
            var scheduledAppointments = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Scheduled);
            var attendedAppointments = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Attended);
            var cancelledAppointments = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Cancelled);

            return new Dictionary<string, int>
            {
                { "Total", totalAppointments },
                { "Programadas", scheduledAppointments },
                { "Atendidas", attendedAppointments },
                { "Canceladas", cancelledAppointments }
            };
        }

        private void ValidateAppointmentData(Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment), "La cita no puede ser nula");

            // Validar IDs
            if (!ValidationHelper.IsValidId(appointment.PatientId))
                throw new ArgumentException("El ID del paciente es inválido");

            if (!ValidationHelper.IsValidId(appointment.DoctorId))
                throw new ArgumentException("El ID del médico es inválido");

            // Validar fecha
            if (appointment.AppointmentDate == default)
                throw new ArgumentException("La fecha de la cita es requerida");

            if (!ValidationHelper.IsValidDate(appointment.AppointmentDate))
                throw new ArgumentException("La fecha de la cita debe ser hoy o en el futuro, máximo 2 años");

            // Validar hora
            if (appointment.AppointmentTime == default)
                throw new ArgumentException("La hora de la cita es requerida");

            if (!ValidationHelper.IsValidTime(appointment.AppointmentTime))
                throw new ArgumentException("La hora de la cita debe estar entre las 6:00 AM y las 10:00 PM");

            // Validar notas si existen
            if (!string.IsNullOrWhiteSpace(appointment.Notes))
            {
                if (!ValidationHelper.IsValidNotes(appointment.Notes))
                    throw new ArgumentException("Las notas contienen caracteres no permitidos o exceden el límite de 500 caracteres");
            }

            // Validar que la fecha y hora combinadas no sean en el pasado
            var appointmentDateTime = appointment.AppointmentDate.Date.Add(appointment.AppointmentTime);
            if (appointmentDateTime <= DateTime.Now)
                throw new ArgumentException("No se pueden programar citas en fechas u horas pasadas");
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByDoctorAsync(int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<bool> HasDoctorConflictAsync(int doctorId, DateTime date, TimeSpan time, int? excludeAppointmentId = null)
        {
            var query = _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                           a.AppointmentDate.Date == date.Date &&
                           a.AppointmentTime == time &&
                           a.Status != AppointmentStatus.Cancelled);

            if (excludeAppointmentId.HasValue)
            {
                query = query.Where(a => a.Id != excludeAppointmentId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> HasPatientConflictAsync(int patientId, DateTime date, TimeSpan time, int? excludeAppointmentId = null)
        {
            var query = _context.Appointments
                .Where(a => a.PatientId == patientId &&
                           a.AppointmentDate.Date == date.Date &&
                           a.AppointmentTime == time &&
                           a.Status != AppointmentStatus.Cancelled);

            if (excludeAppointmentId.HasValue)
            {
                query = query.Where(a => a.Id != excludeAppointmentId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
