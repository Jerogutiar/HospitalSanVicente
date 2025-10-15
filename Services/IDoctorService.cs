using HospitalSanVicente.Models;

namespace HospitalSanVicente.Services
{
    /// <summary>
    /// Interfaz que define el contrato para el servicio de gestión de médicos.
    /// Proporciona abstracción para operaciones CRUD y consultas específicas de médicos.
    /// </summary>
    public interface IDoctorService
    {
        /// <summary>
        /// Obtiene un médico por su ID único.
        /// </summary>
        /// <param name="id">ID del médico a buscar</param>
        /// <returns>El médico encontrado o null si no existe</returns>
        Task<Doctor?> GetDoctorByIdAsync(int id);

        /// <summary>
        /// Obtiene todos los médicos registrados en el sistema.
        /// </summary>
        /// <returns>Lista de todos los médicos</returns>
        Task<List<Doctor>> GetAllDoctorsAsync();

        /// <summary>
        /// Busca médicos por término de búsqueda (nombre, documento, especialidad o email).
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de médicos que coinciden con el término</returns>
        Task<List<Doctor>> SearchDoctorsAsync(string searchTerm);

        /// <summary>
        /// Obtiene todos los médicos de una especialidad específica.
        /// </summary>
        /// <param name="specialty">Especialidad médica</param>
        /// <returns>Lista de médicos de la especialidad</returns>
        Task<List<Doctor>> GetDoctorsBySpecialtyAsync(string specialty);

        /// <summary>
        /// Obtiene todas las especialidades médicas disponibles en el sistema.
        /// </summary>
        /// <returns>Lista de especialidades únicas</returns>
        Task<List<string>> GetAllSpecialtiesAsync();

        /// <summary>
        /// Crea un nuevo médico en el sistema.
        /// </summary>
        /// <param name="doctor">Datos del médico a crear</param>
        /// <returns>El médico creado con ID asignado</returns>
        Task<Doctor> CreateDoctorAsync(Doctor doctor);

        /// <summary>
        /// Actualiza los datos de un médico existente.
        /// </summary>
        /// <param name="id">ID del médico a actualizar</param>
        /// <param name="doctor">Nuevos datos del médico</param>
        /// <returns>El médico actualizado</returns>
        Task<Doctor> UpdateDoctorAsync(int id, Doctor doctor);

        /// <summary>
        /// Elimina un médico del sistema.
        /// </summary>
        /// <param name="id">ID del médico a eliminar</param>
        /// <returns>True si se eliminó exitosamente, false en caso contrario</returns>
        Task<bool> DeleteDoctorAsync(int id);

        /// <summary>
        /// Obtiene todas las citas de un médico específico.
        /// </summary>
        /// <param name="doctorId">ID del médico</param>
        /// <returns>Lista de citas del médico</returns>
        Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorId);

        /// <summary>
        /// Verifica si existe un médico con el documento especificado.
        /// </summary>
        /// <param name="document">Documento a verificar</param>
        /// <param name="excludeId">ID a excluir de la búsqueda (para actualizaciones)</param>
        /// <returns>True si el documento ya existe, false en caso contrario</returns>
        Task<bool> DoctorExistsByDocumentAsync(string document, int? excludeId = null);

        /// <summary>
        /// Verifica si existe un médico con la combinación nombre-especialidad especificada.
        /// </summary>
        /// <param name="name">Nombre del médico</param>
        /// <param name="specialty">Especialidad del médico</param>
        /// <param name="excludeId">ID a excluir de la búsqueda (para actualizaciones)</param>
        /// <returns>True si la combinación ya existe, false en caso contrario</returns>
        Task<bool> DoctorExistsByNameAndSpecialtyAsync(string name, string specialty, int? excludeId = null);
    }
}
