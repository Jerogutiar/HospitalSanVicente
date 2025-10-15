using HospitalSanVicente.Models;

namespace HospitalSanVicente.Services
{
    /// <summary>
    /// Interfaz que define el contrato para el servicio de interfaz de usuario del hospital.
    /// Proporciona abstracción para todas las operaciones de UI y presentación de datos.
    /// </summary>
    public interface IHospitalUIService
    {
        #region Métodos de Presentación

        /// <summary>
        /// Muestra el menú principal del sistema.
        /// </summary>
        void ShowMainMenu();

        /// <summary>
        /// Muestra el menú de gestión de pacientes.
        /// </summary>
        void ShowPatientManagementMenu();

        /// <summary>
        /// Muestra el menú de gestión de médicos.
        /// </summary>
        void ShowDoctorManagementMenu();

        /// <summary>
        /// Muestra el menú de gestión de citas médicas.
        /// </summary>
        void ShowAppointmentManagementMenu();

        /// <summary>
        /// Muestra el menú de historial de correos electrónicos.
        /// </summary>
        void ShowEmailHistoryMenu();

        #endregion

        #region Métodos de Gestión de Pacientes

        /// <summary>
        /// Crea un nuevo paciente a través de la interfaz de usuario.
        /// </summary>
        /// <returns>El paciente creado</returns>
        Task<Patient> CreatePatientAsync();

        /// <summary>
        /// Actualiza un paciente existente a través de la interfaz de usuario.
        /// </summary>
        /// <returns>El paciente actualizado o null si no se encontró</returns>
        Task<Patient?> UpdatePatientAsync();

        /// <summary>
        /// Busca pacientes a través de la interfaz de usuario.
        /// </summary>
        /// <returns>Lista de pacientes encontrados</returns>
        Task<List<Patient>> SearchPatientsAsync();

        /// <summary>
        /// Lista todos los pacientes a través de la interfaz de usuario.
        /// </summary>
        /// <returns>Lista de todos los pacientes</returns>
        Task<List<Patient>> ListAllPatientsAsync();

        /// <summary>
        /// Muestra las citas de un paciente específico.
        /// </summary>
        /// <returns>Lista de citas del paciente</returns>
        Task<List<Appointment>> ShowPatientAppointmentsAsync();

        /// <summary>
        /// Elimina un paciente a través de la interfaz de usuario.
        /// </summary>
        /// <returns>True si se eliminó exitosamente</returns>
        Task<bool> DeletePatientAsync();

        #endregion

        #region Métodos de Gestión de Médicos

        /// <summary>
        /// Crea un nuevo médico a través de la interfaz de usuario.
        /// </summary>
        /// <returns>El médico creado</returns>
        Task<Doctor> CreateDoctorAsync();

        /// <summary>
        /// Actualiza un médico existente a través de la interfaz de usuario.
        /// </summary>
        /// <returns>El médico actualizado o null si no se encontró</returns>
        Task<Doctor?> UpdateDoctorAsync();

        /// <summary>
        /// Busca médicos a través de la interfaz de usuario.
        /// </summary>
        /// <returns>Lista de médicos encontrados</returns>
        Task<List<Doctor>> SearchDoctorsAsync();

        /// <summary>
        /// Lista todos los médicos a través de la interfaz de usuario.
        /// </summary>
        /// <returns>Lista de todos los médicos</returns>
        Task<List<Doctor>> ListAllDoctorsAsync();

        /// <summary>
        /// Lista médicos por especialidad a través de la interfaz de usuario.
        /// </summary>
        /// <returns>Lista de médicos de la especialidad seleccionada</returns>
        Task<List<Doctor>> ListDoctorsBySpecialtyAsync();

        /// <summary>
        /// Muestra las citas de un médico específico.
        /// </summary>
        /// <returns>Lista de citas del médico</returns>
        Task<List<Appointment>> ShowDoctorAppointmentsAsync();

        /// <summary>
        /// Elimina un médico a través de la interfaz de usuario.
        /// </summary>
        /// <returns>True si se eliminó exitosamente</returns>
        Task<bool> DeleteDoctorAsync();

        #endregion

        #region Métodos de Gestión de Citas

        /// <summary>
        /// Crea una nueva cita a través de la interfaz de usuario.
        /// </summary>
        /// <returns>La cita creada</returns>
        Task<Appointment> CreateAppointmentAsync();

        /// <summary>
        /// Actualiza una cita existente a través de la interfaz de usuario.
        /// </summary>
        /// <returns>La cita actualizada o null si no se encontró</returns>
        Task<Appointment?> UpdateAppointmentAsync();

        /// <summary>
        /// Cancela una cita a través de la interfaz de usuario.
        /// </summary>
        /// <returns>True si se canceló exitosamente</returns>
        Task<bool> CancelAppointmentAsync();

        /// <summary>
        /// Marca una cita como atendida a través de la interfaz de usuario.
        /// </summary>
        /// <returns>True si se marcó exitosamente</returns>
        Task<bool> MarkAppointmentAsAttendedAsync();

        /// <summary>
        /// Lista todas las citas a través de la interfaz de usuario.
        /// </summary>
        /// <returns>Lista de todas las citas</returns>
        Task<List<Appointment>> ListAllAppointmentsAsync();

        /// <summary>
        /// Lista citas por fecha a través de la interfaz de usuario.
        /// </summary>
        /// <returns>Lista de citas de la fecha seleccionada</returns>
        Task<List<Appointment>> ListAppointmentsByDateAsync();

        /// <summary>
        /// Lista citas por estado a través de la interfaz de usuario.
        /// </summary>
        /// <returns>Lista de citas del estado seleccionado</returns>
        Task<List<Appointment>> ListAppointmentsByStatusAsync();

        /// <summary>
        /// Elimina una cita a través de la interfaz de usuario.
        /// </summary>
        /// <returns>True si se eliminó exitosamente</returns>
        Task<bool> DeleteAppointmentAsync();

        #endregion

        #region Métodos de Gestión de Correos

        /// <summary>
        /// Muestra todo el historial de correos a través de la interfaz de usuario.
        /// </summary>
        /// <returns>Lista de todos los logs de correos</returns>
        Task<List<EmailLog>> ShowAllEmailHistoryAsync();

        /// <summary>
        /// Muestra el historial de correos por estado a través de la interfaz de usuario.
        /// </summary>
        /// <param name="status">Estado de los correos a mostrar</param>
        /// <returns>Lista de logs de correos del estado especificado</returns>
        Task<List<EmailLog>> ShowEmailHistoryByStatusAsync(EmailStatus status);

        /// <summary>
        /// Prueba la configuración de correo a través de la interfaz de usuario.
        /// </summary>
        /// <returns>True si la prueba fue exitosa</returns>
        Task<bool> TestEmailConfigurationAsync();

        #endregion

        #region Métodos de Estadísticas

        /// <summary>
        /// Muestra las estadísticas del sistema a través de la interfaz de usuario.
        /// </summary>
        Task ShowStatisticsAsync();

        #endregion

        #region Métodos de Entrada de Datos

        /// <summary>
        /// Obtiene los datos de entrada para un paciente.
        /// </summary>
        /// <returns>Objeto Patient con los datos ingresados</returns>
        Patient GetPatientInput();

        /// <summary>
        /// Obtiene los datos de entrada para un médico.
        /// </summary>
        /// <returns>Objeto Doctor con los datos ingresados</returns>
        Doctor GetDoctorInput();

        /// <summary>
        /// Obtiene los datos de entrada para una cita.
        /// </summary>
        /// <returns>Objeto Appointment con los datos ingresados</returns>
        Appointment GetAppointmentInput();

        /// <summary>
        /// Obtiene un ID de entrada del usuario.
        /// </summary>
        /// <param name="entityName">Nombre de la entidad para el mensaje</param>
        /// <returns>ID ingresado por el usuario</returns>
        int GetIdInput(string entityName);

        /// <summary>
        /// Obtiene un término de búsqueda del usuario.
        /// </summary>
        /// <returns>Término de búsqueda ingresado</returns>
        string GetSearchTermInput();

        /// <summary>
        /// Obtiene una fecha de entrada del usuario.
        /// </summary>
        /// <returns>Fecha ingresada por el usuario</returns>
        DateTime GetDateInput();

        /// <summary>
        /// Obtiene una confirmación del usuario.
        /// </summary>
        /// <param name="message">Mensaje de confirmación</param>
        /// <returns>True si el usuario confirma, false en caso contrario</returns>
        bool GetConfirmationInput(string message);

        #endregion

        #region Métodos de Presentación de Datos

        /// <summary>
        /// Muestra los datos de un paciente en la consola.
        /// </summary>
        /// <param name="patient">Paciente a mostrar</param>
        void DisplayPatient(Patient patient);

        /// <summary>
        /// Muestra los datos de un médico en la consola.
        /// </summary>
        /// <param name="doctor">Médico a mostrar</param>
        void DisplayDoctor(Doctor doctor);

        /// <summary>
        /// Muestra los datos de una cita en la consola.
        /// </summary>
        /// <param name="appointment">Cita a mostrar</param>
        void DisplayAppointment(Appointment appointment);

        /// <summary>
        /// Muestra los datos de un log de correo en la consola.
        /// </summary>
        /// <param name="emailLog">Log de correo a mostrar</param>
        void DisplayEmailLog(EmailLog emailLog);

        #endregion
    }
}
