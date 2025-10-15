using HospitalSanVicente.Models;

namespace HospitalSanVicente.Services
{
    /// <summary>
    /// Interfaz que define el contrato para el servicio de gestión de pacientes.
    /// Proporciona abstracción para operaciones CRUD y consultas específicas de pacientes.
    /// </summary>
    public interface IPatientService
    {
        /// <summary>
        /// Obtiene un paciente por su ID único.
        /// </summary>
        /// <param name="id">ID del paciente a buscar</param>
        /// <returns>El paciente encontrado o null si no existe</returns>
        Task<Patient?> GetPatientByIdAsync(int id);

        /// <summary>
        /// Obtiene todos los pacientes registrados en el sistema.
        /// </summary>
        /// <returns>Lista de todos los pacientes</returns>
        Task<List<Patient>> GetAllPatientsAsync();

        /// <summary>
        /// Busca pacientes por término de búsqueda (nombre, documento o email).
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de pacientes que coinciden con el término</returns>
        Task<List<Patient>> SearchPatientsAsync(string searchTerm);

        /// <summary>
        /// Crea un nuevo paciente en el sistema.
        /// </summary>
        /// <param name="patient">Datos del paciente a crear</param>
        /// <returns>El paciente creado con ID asignado</returns>
        Task<Patient> CreatePatientAsync(Patient patient);

        /// <summary>
        /// Actualiza los datos de un paciente existente.
        /// </summary>
        /// <param name="id">ID del paciente a actualizar</param>
        /// <param name="patient">Nuevos datos del paciente</param>
        /// <returns>El paciente actualizado</returns>
        Task<Patient> UpdatePatientAsync(int id, Patient patient);

        /// <summary>
        /// Elimina un paciente del sistema.
        /// </summary>
        /// <param name="id">ID del paciente a eliminar</param>
        /// <returns>True si se eliminó exitosamente, false en caso contrario</returns>
        Task<bool> DeletePatientAsync(int id);

        /// <summary>
        /// Obtiene todas las citas de un paciente específico.
        /// </summary>
        /// <param name="patientId">ID del paciente</param>
        /// <returns>Lista de citas del paciente</returns>
        Task<List<Appointment>> GetPatientAppointmentsAsync(int patientId);

        /// <summary>
        /// Verifica si existe un paciente con el documento especificado.
        /// </summary>
        /// <param name="document">Documento a verificar</param>
        /// <param name="excludeId">ID a excluir de la búsqueda (para actualizaciones)</param>
        /// <returns>True si el documento ya existe, false en caso contrario</returns>
        Task<bool> PatientExistsByDocumentAsync(string document, int? excludeId = null);
    }
}
