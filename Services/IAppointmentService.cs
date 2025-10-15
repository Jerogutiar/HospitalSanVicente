using HospitalSanVicente.Models;

namespace HospitalSanVicente.Services
{
    /// <summary>
    /// Interfaz que define el contrato para el servicio de gestión de citas médicas.
    /// Proporciona abstracción para operaciones CRUD y consultas específicas de citas.
    /// </summary>
    public interface IAppointmentService
    {
        /// <summary>
        /// Obtiene una cita por su ID único.
        /// </summary>
        /// <param name="id">ID de la cita a buscar</param>
        /// <returns>La cita encontrada o null si no existe</returns>
        Task<Appointment?> GetAppointmentByIdAsync(int id);

        /// <summary>
        /// Obtiene todas las citas registradas en el sistema.
        /// </summary>
        /// <returns>Lista de todas las citas</returns>
        Task<List<Appointment>> GetAllAppointmentsAsync();

        /// <summary>
        /// Obtiene todas las citas de una fecha específica.
        /// </summary>
        /// <param name="date">Fecha de las citas</param>
        /// <returns>Lista de citas de la fecha</returns>
        Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date);

        /// <summary>
        /// Obtiene todas las citas con un estado específico.
        /// </summary>
        /// <param name="status">Estado de las citas</param>
        /// <returns>Lista de citas con el estado</returns>
        Task<List<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status);

        /// <summary>
        /// Obtiene todas las citas de un paciente específico.
        /// </summary>
        /// <param name="patientId">ID del paciente</param>
        /// <returns>Lista de citas del paciente</returns>
        Task<List<Appointment>> GetAppointmentsByPatientAsync(int patientId);

        /// <summary>
        /// Obtiene todas las citas de un médico específico.
        /// </summary>
        /// <param name="doctorId">ID del médico</param>
        /// <returns>Lista de citas del médico</returns>
        Task<List<Appointment>> GetAppointmentsByDoctorAsync(int doctorId);

        /// <summary>
        /// Crea una nueva cita en el sistema.
        /// </summary>
        /// <param name="appointment">Datos de la cita a crear</param>
        /// <returns>La cita creada con ID asignado</returns>
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);

        /// <summary>
        /// Actualiza los datos de una cita existente.
        /// </summary>
        /// <param name="id">ID de la cita a actualizar</param>
        /// <param name="appointment">Nuevos datos de la cita</param>
        /// <returns>La cita actualizada</returns>
        Task<Appointment> UpdateAppointmentAsync(int id, Appointment appointment);

        /// <summary>
        /// Cancela una cita existente.
        /// </summary>
        /// <param name="id">ID de la cita a cancelar</param>
        /// <returns>True si se canceló exitosamente, false en caso contrario</returns>
        Task<bool> CancelAppointmentAsync(int id);

        /// <summary>
        /// Marca una cita como atendida.
        /// </summary>
        /// <param name="id">ID de la cita a marcar como atendida</param>
        /// <returns>True si se marcó exitosamente, false en caso contrario</returns>
        Task<bool> MarkAppointmentAsAttendedAsync(int id);

        /// <summary>
        /// Elimina una cita del sistema.
        /// </summary>
        /// <param name="id">ID de la cita a eliminar</param>
        /// <returns>True si se eliminó exitosamente, false en caso contrario</returns>
        Task<bool> DeleteAppointmentAsync(int id);

        /// <summary>
        /// Obtiene estadísticas de las citas del sistema.
        /// </summary>
        /// <returns>Diccionario con estadísticas de citas</returns>
        Task<Dictionary<string, int>> GetAppointmentStatisticsAsync();

        /// <summary>
        /// Verifica si existe un conflicto de horario para un médico en una fecha y hora específicas.
        /// </summary>
        /// <param name="doctorId">ID del médico</param>
        /// <param name="date">Fecha de la cita</param>
        /// <param name="time">Hora de la cita</param>
        /// <param name="excludeAppointmentId">ID de cita a excluir (para actualizaciones)</param>
        /// <returns>True si hay conflicto, false en caso contrario</returns>
        Task<bool> HasDoctorConflictAsync(int doctorId, DateTime date, TimeSpan time, int? excludeAppointmentId = null);

        /// <summary>
        /// Verifica si existe un conflicto de horario para un paciente en una fecha y hora específicas.
        /// </summary>
        /// <param name="patientId">ID del paciente</param>
        /// <param name="date">Fecha de la cita</param>
        /// <param name="time">Hora de la cita</param>
        /// <param name="excludeAppointmentId">ID de cita a excluir (para actualizaciones)</param>
        /// <returns>True si hay conflicto, false en caso contrario</returns>
        Task<bool> HasPatientConflictAsync(int patientId, DateTime date, TimeSpan time, int? excludeAppointmentId = null);
    }
}
