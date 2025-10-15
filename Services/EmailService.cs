using Microsoft.EntityFrameworkCore;
using HospitalSanVicente.Data;
using HospitalSanVicente.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace HospitalSanVicente.Services
{
    public class EmailService : IEmailService
    {
        private readonly HospitalDbContext _context;
        
        // Configuración SMTP para Gmail
        private readonly string _smtpHost = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "hospitalsanvicenteriwi@gmail.com"; // Email del hospital
        private readonly string _smtpPassword = "qjws fxmc zxcd bdnm"; // Clave de aplicación de Gmail
        private readonly string _fromName = "Hospital San Vicente";
        private readonly string _fromEmail = "hospitalsanvicenteriwi@gmail.com"; // Email del hospital

        public EmailService(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SendAppointmentConfirmationAsync(Appointment appointment)
        {
            try
            {
                var subject = "Confirmación de Cita Médica - Hospital San Vicente";
                var body = GenerateAppointmentEmailBody(appointment);
                var recipientEmail = appointment.Patient?.Email ?? "email@ejemplo.com";

                var emailLog = new EmailLog
                {
                    AppointmentId = appointment.Id,
                    RecipientEmail = recipientEmail,
                    Subject = subject,
                    Body = body,
                    Status = EmailStatus.NotSent
                };

                _context.EmailLogs.Add(emailLog);
                await _context.SaveChangesAsync();

                // Enviar correo real usando MailKit
                var success = await SendEmailWithMailKit(recipientEmail, subject, body);

                emailLog.Status = success ? EmailStatus.Sent : EmailStatus.Failed;
                emailLog.SentAt = DateTime.Now;

                if (!success)
                {
                    emailLog.ErrorMessage = "Error al enviar el correo electrónico";
                }

                await _context.SaveChangesAsync();
                return success;
            }
            catch (Exception ex)
            {
                try
                {
                    // Crear un log de error básico sin formateo complejo
                    var emailLog = new EmailLog
                    {
                        AppointmentId = appointment.Id,
                        RecipientEmail = appointment.Patient?.Email ?? "email@ejemplo.com",
                        Subject = "Confirmación de Cita Médica - Hospital San Vicente",
                        Body = $"Cita médica confirmada para el paciente {appointment.Patient?.Name ?? "Paciente"}",
                        Status = EmailStatus.Failed,
                        ErrorMessage = ex.Message,
                        SentAt = DateTime.Now
                    };

                    _context.EmailLogs.Add(emailLog);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    // Si incluso el log de error falla, no hacer nada para no interrumpir el flujo principal
                }

                return false;
            }
        }

        private async Task<bool> SendEmailWithMailKit(string to, string subject, string body)
        {
            try
            {
                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress(_fromName, _fromEmail));
                mensaje.To.Add(MailboxAddress.Parse(to));
                mensaje.Subject = subject;

                mensaje.Body = new TextPart("plain")
                {
                    Text = body
                };

                using var cliente = new SmtpClient();
                
                // Conectar con timeout
                await cliente.ConnectAsync(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                
                // Autenticar
                await cliente.AuthenticateAsync(_smtpUsername, _smtpPassword);
                
                // Enviar
                await cliente.SendAsync(mensaje);
                
                // Desconectar
                await cliente.DisconnectAsync(true);

                Console.WriteLine($"Correo enviado exitosamente a: {to}");
                return true;
            }
            catch (Exception ex)
            {
                // Log detallado del error
                Console.WriteLine($"Error al enviar correo a {to}:");
                Console.WriteLine($"   Tipo: {ex.GetType().Name}");
                Console.WriteLine($"   Mensaje: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"   Error interno: {ex.InnerException.Message}");
                }
                return false;
            }
        }

        private string GenerateAppointmentEmailBody(Appointment appointment)
        {
            try
            {
                // Formatear fecha y hora de manera segura
                var fechaFormateada = appointment.AppointmentDate.ToString("dd/MM/yyyy");
                var horaFormateada = $"{appointment.AppointmentTime.Hours:D2}:{appointment.AppointmentTime.Minutes:D2}";

                return $@"
Estimado/a {appointment.Patient?.Name ?? "Paciente"},

Le confirmamos su cita médica en el Hospital San Vicente:

DETALLES DE LA CITA:
- Fecha: {fechaFormateada}
- Hora: {horaFormateada}
- Médico: Dr. {appointment.Doctor?.Name ?? "Médico"}
- Especialidad: {appointment.Doctor?.Specialty ?? "Especialidad"}
- Estado: {GetStatusInSpanish(appointment.Status)}

INFORMACIÓN DEL PACIENTE:
- Nombre: {appointment.Patient?.Name ?? "Paciente"}
- Documento: {appointment.Patient?.Document ?? "No disponible"}
- Teléfono: {appointment.Patient?.Phone ?? "No disponible"}

NOTAS DEL DOCTOR:
{(!string.IsNullOrEmpty(appointment.Notes) ? appointment.Notes : "Sin notas del doctor")}

INSTRUCCIONES:
- Por favor llegue 15 minutos antes de su cita
- Traiga su documento de identidad
- Si necesita cancelar o reprogramar, contacte al hospital con al menos 24 horas de anticipación

Hospital San Vicente
Teléfono: (555) 123-4567
Email: info@hospitalsanvicente.com

Gracias por confiar en nosotros para su atención médica.
";
            }
            catch (Exception)
            {
                // Si hay error en el formateo, usar valores por defecto
                return $@"
Estimado/a {appointment.Patient?.Name ?? "Paciente"},

Le confirmamos su cita médica en el Hospital San Vicente:

DETALLES DE LA CITA:
- Fecha: {appointment.AppointmentDate:dd/MM/yyyy}
- Hora: {appointment.AppointmentTime.ToString(@"HH\:mm")}
- Médico: Dr. {appointment.Doctor?.Name ?? "Médico"}
- Especialidad: {appointment.Doctor?.Specialty ?? "Especialidad"}
- Estado: {GetStatusInSpanish(appointment.Status)}

Hospital San Vicente
Teléfono: (555) 123-4567
Email: info@hospitalsanvicente.com

Gracias por confiar en nosotros para su atención médica.
";
            }
        }

        private string GetStatusInSpanish(AppointmentStatus status)
        {
            return status switch
            {
                AppointmentStatus.Scheduled => "Programada",
                AppointmentStatus.Attended => "Atendida",
                AppointmentStatus.Cancelled => "Cancelada",
                _ => "Desconocido"
            };
        }

        public async Task<List<EmailLog>> GetEmailHistoryAsync()
        {
            return await _context.EmailLogs
                .Include(e => e.Appointment)
                .ThenInclude(a => a.Patient)
                .Include(e => e.Appointment)
                .ThenInclude(a => a.Doctor)
                .OrderByDescending(e => e.SentAt)
                .ToListAsync();
        }

        public async Task<List<EmailLog>> GetEmailHistoryByStatusAsync(EmailStatus status)
        {
            return await _context.EmailLogs
                .Include(e => e.Appointment)
                .ThenInclude(a => a.Patient)
                .Include(e => e.Appointment)
                .ThenInclude(a => a.Doctor)
                .Where(e => e.Status == status)
                .OrderByDescending(e => e.SentAt)
                .ToListAsync();
        }

        // Método para probar la configuración de correo
        public async Task<bool> TestEmailConfigurationAsync(string testEmail)
        {
            try
            {
                Console.WriteLine("INICIANDO PRUEBA DE CONFIGURACION SMTP");
                Console.WriteLine($"Host SMTP: {_smtpHost}");
                Console.WriteLine($"Puerto: {_smtpPort}");
                Console.WriteLine($"Usuario: {_smtpUsername}");
                Console.WriteLine($"Contraseña: {(_smtpPassword.Length > 0 ? "***" + _smtpPassword.Substring(_smtpPassword.Length - 4) : "NO CONFIGURADA")}");
                Console.WriteLine($"Email origen: {_fromEmail}");
                Console.WriteLine($"Email destino: {testEmail}");
                Console.WriteLine();

                var subject = "Prueba de Configuración - Hospital San Vicente";
                var body = "Este es un correo de prueba para verificar que la configuración SMTP funciona correctamente.";
                
                Console.WriteLine("Creando mensaje...");
                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress(_fromName, _fromEmail));
                mensaje.To.Add(MailboxAddress.Parse(testEmail));
                mensaje.Subject = subject;
                mensaje.Body = new TextPart("plain") { Text = body };

                Console.WriteLine("Conectando al servidor SMTP...");
                using var cliente = new SmtpClient();
                
                Console.WriteLine($"Conectando a {_smtpHost}:{_smtpPort}...");
                await cliente.ConnectAsync(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                Console.WriteLine("Conexion establecida");
                
                Console.WriteLine("Autenticando...");
                await cliente.AuthenticateAsync(_smtpUsername, _smtpPassword);
                Console.WriteLine("Autenticacion exitosa");
                
                Console.WriteLine("Enviando mensaje...");
                await cliente.SendAsync(mensaje);
                Console.WriteLine("Mensaje enviado");
                
                Console.WriteLine("Desconectando...");
                await cliente.DisconnectAsync(true);
                Console.WriteLine("Desconexion completada");
                
                Console.WriteLine("CORREO ENVIADO EXITOSAMENTE!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR EN PRUEBA DE CONFIGURACION:");
                Console.WriteLine($"   Tipo: {ex.GetType().Name}");
                Console.WriteLine($"   Mensaje: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"   Error interno: {ex.InnerException.Message}");
                }
                Console.WriteLine($"   Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<Dictionary<string, int>> GetEmailStatisticsAsync()
        {
            var emailLogs = await _context.EmailLogs.ToListAsync();
            
            return new Dictionary<string, int>
            {
                ["Total"] = emailLogs.Count,
                ["Enviados"] = emailLogs.Count(e => e.Status == EmailStatus.Sent),
                ["NoEnviados"] = emailLogs.Count(e => e.Status == EmailStatus.NotSent),
                ["Fallidos"] = emailLogs.Count(e => e.Status == EmailStatus.Failed)
            };
        }

        public async Task<List<EmailLog>> GetEmailHistoryByAppointmentAsync(int appointmentId)
        {
            return await _context.EmailLogs
                .Where(e => e.AppointmentId == appointmentId)
                .OrderByDescending(e => e.SentAt)
                .ToListAsync();
        }

        public async Task<bool> ResendEmailAsync(int emailLogId)
        {
            var emailLog = await _context.EmailLogs.FindAsync(emailLogId);
            if (emailLog == null)
                return false;

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == emailLog.AppointmentId);

            if (appointment == null)
                return false;

            // Intentar reenviar el correo
            var success = await SendAppointmentConfirmationAsync(appointment);
            
            // Actualizar el log original
            emailLog.Status = success ? EmailStatus.Sent : EmailStatus.Failed;
            emailLog.SentAt = DateTime.Now;
            emailLog.ErrorMessage = success ? null : "Error al reenviar correo";
            
            await _context.SaveChangesAsync();
            return success;
        }

        public async Task<double> GetEmailSuccessRateAsync()
        {
            var emailLogs = await _context.EmailLogs.ToListAsync();
            
            if (!emailLogs.Any())
                return 0.0;
            
            var sentCount = emailLogs.Count(e => e.Status == EmailStatus.Sent);
            return (double)sentCount / emailLogs.Count * 100;
        }
    }
}