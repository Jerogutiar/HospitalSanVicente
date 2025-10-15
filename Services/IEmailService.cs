using HospitalSanVicente.Models;

namespace HospitalSanVicente.Services
{
    /// <summary>
    /// Interfaz que define el contrato para el servicio de gestión de correos electrónicos.
    /// Proporciona abstracción para operaciones de envío de correos y gestión del historial.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Envía un correo de confirmación de cita al paciente.
        /// </summary>
        /// <param name="appointment">Cita para la cual enviar la confirmación</param>
        /// <returns>True si se envió exitosamente, false en caso contrario</returns>
        Task<bool> SendAppointmentConfirmationAsync(Appointment appointment);

        /// <summary>
        /// Obtiene todo el historial de correos electrónicos.
        /// </summary>
        /// <returns>Lista de todos los logs de correos</returns>
        Task<List<EmailLog>> GetEmailHistoryAsync();

        /// <summary>
        /// Obtiene el historial de correos filtrado por estado.
        /// </summary>
        /// <param name="status">Estado de los correos a filtrar</param>
        /// <returns>Lista de logs de correos con el estado especificado</returns>
        Task<List<EmailLog>> GetEmailHistoryByStatusAsync(EmailStatus status);

        /// <summary>
        /// Prueba la configuración de correo enviando un mensaje de prueba.
        /// </summary>
        /// <param name="testEmail">Dirección de correo para la prueba</param>
        /// <returns>True si la prueba fue exitosa, false en caso contrario</returns>
        Task<bool> TestEmailConfigurationAsync(string testEmail);

        /// <summary>
        /// Obtiene estadísticas de los correos electrónicos.
        /// </summary>
        /// <returns>Diccionario con estadísticas de correos</returns>
        Task<Dictionary<string, int>> GetEmailStatisticsAsync();

        /// <summary>
        /// Obtiene el historial de correos de una cita específica.
        /// </summary>
        /// <param name="appointmentId">ID de la cita</param>
        /// <returns>Lista de logs de correos de la cita</returns>
        Task<List<EmailLog>> GetEmailHistoryByAppointmentAsync(int appointmentId);

        /// <summary>
        /// Reenvía un correo que falló anteriormente.
        /// </summary>
        /// <param name="emailLogId">ID del log de correo a reenviar</param>
        /// <returns>True si se reenvió exitosamente, false en caso contrario</returns>
        Task<bool> ResendEmailAsync(int emailLogId);

        /// <summary>
        /// Obtiene la tasa de éxito de envío de correos.
        /// </summary>
        /// <returns>Porcentaje de correos enviados exitosamente</returns>
        Task<double> GetEmailSuccessRateAsync();
    }
}
