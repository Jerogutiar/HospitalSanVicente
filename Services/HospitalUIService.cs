using HospitalSanVicente.Models;
using HospitalSanVicente.Utils;

namespace HospitalSanVicente.Services
{
    public class HospitalUIService : IHospitalUIService
    {
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly IEmailService _emailService;

        public HospitalUIService(IPatientService patientService, IDoctorService doctorService, 
                               IAppointmentService appointmentService, IEmailService emailService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _emailService = emailService;
        }

        #region Métodos de Presentación

        public void ShowMainMenu()
        {
            Console.WriteLine("MENÚ PRINCIPAL");
            Console.WriteLine("1. Gestión de Pacientes");
            Console.WriteLine("2. Gestión de Médicos");
            Console.WriteLine("3. Gestión de Citas Médicas");
            Console.WriteLine("4. Historial de Correos Electrónicos");
            Console.WriteLine("5. Estadísticas del Sistema");
            Console.WriteLine("0. Salir");
            Console.WriteLine();
            Console.Write("Seleccione una opción: ");
        }

        public void ShowPatientManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("GESTIÓN DE PACIENTES");
            Console.WriteLine("1. Registrar nuevo paciente");
            Console.WriteLine("2. Editar paciente");
            Console.WriteLine("3. Buscar paciente");
            Console.WriteLine("4. Listar todos los pacientes");
            Console.WriteLine("5. Ver citas de un paciente");
            Console.WriteLine("6. Eliminar paciente");
            Console.WriteLine("0. Volver al menú principal");
            Console.WriteLine();
            Console.Write("Seleccione una opción: ");
        }

        public void ShowDoctorManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("GESTIÓN DE MÉDICOS");
            Console.WriteLine("1. Registrar nuevo médico");
            Console.WriteLine("2. Editar médico");
            Console.WriteLine("3. Buscar médico");
            Console.WriteLine("4. Listar todos los médicos");
            Console.WriteLine("5. Listar médicos por especialidad");
            Console.WriteLine("6. Ver citas de un médico");
            Console.WriteLine("7. Eliminar médico");
            Console.WriteLine("0. Volver al menú principal");
            Console.WriteLine();
            Console.Write("Seleccione una opción: ");
        }

        public void ShowAppointmentManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("GESTIÓN DE CITAS MÉDICAS");
            Console.WriteLine("1. Agendar nueva cita");
            Console.WriteLine("2. Editar cita");
            Console.WriteLine("3. Cancelar cita");
            Console.WriteLine("4. Marcar cita como atendida");
            Console.WriteLine("5. Listar todas las citas");
            Console.WriteLine("6. Listar citas por fecha");
            Console.WriteLine("7. Listar citas por estado");
            Console.WriteLine("8. Ver citas de un paciente");
            Console.WriteLine("9. Ver citas de un médico");
            Console.WriteLine("10. Eliminar cita");
            Console.WriteLine("0. Volver al menú principal");
            Console.WriteLine();
            Console.Write("Seleccione una opción: ");
        }

        public void ShowEmailHistoryMenu()
        {
            Console.Clear();
            Console.WriteLine("HISTORIAL DE CORREOS ELECTRÓNICOS");
            Console.WriteLine("1. Ver todo el historial");
            Console.WriteLine("2. Ver correos enviados");
            Console.WriteLine("3. Ver correos no enviados");
            Console.WriteLine("4. Ver correos fallidos");
            Console.WriteLine("5. Probar configuración de email");
            Console.WriteLine("0. Volver al menú principal");
            Console.WriteLine();
            Console.Write("Seleccione una opción: ");
        }

        #endregion

        #region Métodos de Entrada de Datos

        public Patient GetPatientInput()
        {
            var patient = new Patient();

            // Validar nombre
            while (true)
            {
                Console.Write("Nombre completo: ");
                var name = Console.ReadLine()?.Trim() ?? "";
                
                if (ValidationHelper.IsValidName(name))
                {
                    patient.Name = name;
                    break;
                }
                else
                {
                    Console.WriteLine("Nombre inválido. Inténtelo de nuevo.");
                }
            }

            // Validar documento
            while (true)
            {
                Console.Write("Documento de identidad: ");
                var document = Console.ReadLine()?.Trim() ?? "";
                
                if (ValidationHelper.IsValidDocument(document))
                {
                    patient.Document = document;
                    break;
                }
                else
                {
                    Console.WriteLine("Documento inválido. Solo números. Inténtelo de nuevo.");
                }
            }

            // Validar edad
            while (true)
            {
                Console.Write("Edad: ");
                var ageInput = Console.ReadLine()?.Trim() ?? "";
                
                if (int.TryParse(ageInput, out int age) && ValidationHelper.IsValidAge(age))
                {
                    patient.Age = age;
                    break;
                }
                else
                {
                    Console.WriteLine("Edad inválida. Inténtelo de nuevo.");
                }
            }

            // Validar teléfono
            while (true)
            {
                Console.Write("Teléfono: ");
                var phone = Console.ReadLine()?.Trim() ?? "";
                
                if (ValidationHelper.IsValidPhone(phone))
                {
                    patient.Phone = phone;
                    break;
                }
                else
                {
                    Console.WriteLine("Teléfono inválido. Inténtelo de nuevo.");
                }
            }

            // Validar email
            while (true)
            {
                Console.Write("Correo electrónico: ");
                var email = Console.ReadLine()?.Trim() ?? "";
                
                if (ValidationHelper.IsValidEmail(email))
                {
                    patient.Email = email;
                    break;
                }
                else
                {
                    Console.WriteLine("Correo electrónico inválido. Inténtelo de nuevo.");
                }
            }

            return patient;
        }

        public Doctor GetDoctorInput()
        {
            var doctor = new Doctor();

            // Validar nombre
            while (true)
            {
                Console.Write("Nombre completo: ");
                var name = Console.ReadLine()?.Trim() ?? "";
                
                if (ValidationHelper.IsValidName(name))
                {
                    doctor.Name = name;
                    break;
                }
                else
                {
                    Console.WriteLine("Nombre inválido. Inténtelo de nuevo.");
                }
            }

            // Validar documento
            while (true)
            {
                Console.Write("Documento de identidad: ");
                var document = Console.ReadLine()?.Trim() ?? "";
                
                if (ValidationHelper.IsValidDocument(document))
                {
                    doctor.Document = document;
                    break;
                }
                else
                {
                    Console.WriteLine("Documento inválido. Solo números. Inténtelo de nuevo.");
                }
            }

            // Validar especialidad
            while (true)
            {
                Console.Write("Especialidad médica: ");
                var specialty = Console.ReadLine()?.Trim() ?? "";
                
                if (ValidationHelper.IsValidSpecialty(specialty))
                {
                    doctor.Specialty = specialty;
                    break;
                }
                else
                {
                    Console.WriteLine("Especialidad inválida. Inténtelo de nuevo.");
                }
            }

            // Validar teléfono
            while (true)
            {
                Console.Write("Teléfono: ");
                var phone = Console.ReadLine()?.Trim() ?? "";
                
                if (ValidationHelper.IsValidPhone(phone))
                {
                    doctor.Phone = phone;
                    break;
                }
                else
                {
                    Console.WriteLine("Teléfono inválido. Inténtelo de nuevo.");
                }
            }

            // Validar email
            while (true)
            {
                Console.Write("Correo electrónico: ");
                var email = Console.ReadLine()?.Trim() ?? "";
                
                if (ValidationHelper.IsValidEmail(email))
                {
                    doctor.Email = email;
                    break;
                }
                else
                {
                    Console.WriteLine("Correo electrónico inválido. Inténtelo de nuevo.");
                }
            }

            return doctor;
        }

        public Appointment GetAppointmentInput()
        {
            var appointment = new Appointment();

            Console.Write("ID del paciente: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                appointment.PatientId = patientId;
            }
            else
            {
                throw new ArgumentException("ID de paciente inválido.");
            }

            Console.Write("ID del médico: ");
            if (int.TryParse(Console.ReadLine(), out int doctorId))
            {
                appointment.DoctorId = doctorId;
            }
            else
            {
                throw new ArgumentException("ID de médico inválido.");
            }

            // Validar fecha
            while (true)
            {
                Console.Write("Fecha de la cita (dd/mm/yyyy): ");
                var dateInput = Console.ReadLine()?.Trim() ?? "";
                
                if (ValidationHelper.TryParseDate(dateInput, out DateTime date))
                {
                    appointment.AppointmentDate = date;
                    break;
                }
                else
                {
                    Console.WriteLine("Fecha inválida. Use el formato dd/mm/yyyy (ejemplo: 16/10/2025) y asegúrese de que sea hoy o en el futuro.");
                }
            }

            // Validar hora
            while (true)
            {
                Console.Write("Hora de la cita (hh:mm): ");
                var timeInput = Console.ReadLine()?.Trim() ?? "";
                
                if (ValidationHelper.TryParseTime(timeInput, out TimeSpan time))
                {
                    appointment.AppointmentTime = time;
                    break;
                }
                else
                {
                    Console.WriteLine("Hora inválida. Use el formato hh:mm (ejemplo: 10:30, 14:15) y asegúrese de que esté entre 06:00 y 22:00.");
                }
            }

            // Validar notas (opcional)
            while (true)
            {
                Console.Write("Notas del doctor (opcional): ");
                var notesInput = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrWhiteSpace(notesInput) || ValidationHelper.IsValidNotes(notesInput))
                {
                    appointment.Notes = notesInput;
                    break;
                }
                else
                {
                    Console.WriteLine("Las notas contienen caracteres no permitidos o exceden el límite de 500 caracteres.");
                }
            }

            return appointment;
        }

        public int GetIdInput(string entityName)
        {
            Console.Write($"Ingrese el ID del {entityName}: ");
            if (int.TryParse(Console.ReadLine(), out int id) && ValidationHelper.IsValidId(id))
            {
                return id;
            }
            else
            {
                throw new ArgumentException("ID inválido.");
            }
        }

        public string GetSearchTermInput()
        {
            Console.Write("Ingrese el término de búsqueda: ");
            var searchTerm = Console.ReadLine()?.Trim() ?? "";
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Término de búsqueda no puede estar vacío.");
            }
            
            return searchTerm;
        }

        public DateTime GetDateInput()
        {
            Console.Write("Ingrese la fecha (dd/mm/yyyy): ");
            if (ValidationHelper.TryParseDate(Console.ReadLine()?.Trim() ?? "", out DateTime date))
            {
                return date;
            }
            else
            {
                throw new ArgumentException("Fecha inválida. Use el formato dd/mm/yyyy");
            }
        }

        public bool GetConfirmationInput(string message)
        {
            Console.Write($"{message} (s/n): ");
            var confirmation = Console.ReadLine();
            return confirmation?.ToLower() == "s";
        }

        #endregion

        #region Métodos de Presentación de Datos

        public void DisplayPatient(Patient patient)
        {
            Console.WriteLine($"ID: {patient.Id}");
            Console.WriteLine($"Nombre: {patient.Name}");
            Console.WriteLine($"Documento: {patient.Document}");
            Console.WriteLine($"Edad: {patient.Age}");
            Console.WriteLine($"Teléfono: {patient.Phone}");
            Console.WriteLine($"Email: {patient.Email}");
            Console.WriteLine($"Citas: {patient.Appointments.Count}");
            Console.WriteLine("---");
        }

        public void DisplayDoctor(Doctor doctor)
        {
            Console.WriteLine($"ID: {doctor.Id}");
            Console.WriteLine($"Nombre: {doctor.Name}");
            Console.WriteLine($"Documento: {doctor.Document}");
            Console.WriteLine($"Especialidad: {doctor.Specialty}");
            Console.WriteLine($"Teléfono: {doctor.Phone}");
            Console.WriteLine($"Email: {doctor.Email}");
            Console.WriteLine($"Citas: {doctor.Appointments.Count}");
            Console.WriteLine("---");
        }

        public void DisplayAppointment(Appointment appointment)
        {
            Console.WriteLine($"ID: {appointment.Id}");
            Console.WriteLine($"Paciente: {appointment.Patient?.Name ?? "N/A"}");
            Console.WriteLine($"Médico: Dr. {appointment.Doctor?.Name ?? "N/A"}");
            Console.WriteLine($"Fecha: {FormatDateSafely(appointment.AppointmentDate)}");
            Console.WriteLine($"Hora: {FormatTimeSafely(appointment.AppointmentTime)}");
            Console.WriteLine($"Estado: {GetStatusInSpanish(appointment.Status)}");
            if (!string.IsNullOrWhiteSpace(appointment.Notes))
                Console.WriteLine($"Notas del doctor: {appointment.Notes ?? "Sin notas"}");
            Console.WriteLine("---");
        }

        public void DisplayEmailLog(EmailLog emailLog)
        {
            Console.WriteLine($"ID: {emailLog.Id}");
            Console.WriteLine($"Cita ID: {emailLog.AppointmentId}");
            Console.WriteLine($"Destinatario: {emailLog.RecipientEmail}");
            Console.WriteLine($"Asunto: {emailLog.Subject}");
            Console.WriteLine($"Estado: {GetEmailStatusInSpanish(emailLog.Status)}");
            Console.WriteLine($"Enviado: {FormatDateSafely(emailLog.SentAt)}");
            if (!string.IsNullOrWhiteSpace(emailLog.ErrorMessage))
                Console.WriteLine($"Error: {emailLog.ErrorMessage}");
            Console.WriteLine("---");
        }

        #endregion

        #region Métodos de Lógica de Negocio

        public async Task<Patient> CreatePatientAsync()
        {
            Console.Clear();
            Console.WriteLine("REGISTRAR NUEVO PACIENTE");

            var patient = GetPatientInput();
            var createdPatient = await _patientService.CreatePatientAsync(patient);
            Console.WriteLine($"\nPaciente registrado exitosamente con ID: {createdPatient.Id}");
            return createdPatient;
        }

        public async Task<Patient?> UpdatePatientAsync()
        {
            Console.Clear();
            Console.WriteLine("EDITAR PACIENTE");

            var patientId = GetIdInput("paciente a editar");
            var patient = await _patientService.GetPatientByIdAsync(patientId);
            if (patient == null)
            {
                Console.WriteLine("Paciente no encontrado.");
                return null;
            }

            Console.WriteLine($"\nEditando paciente: {patient.Name}");
            Console.WriteLine("Deje en blanco para mantener el valor actual.\n");

            Console.Write($"Nombre completo ({patient.Name}): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                patient.Name = name;

            Console.Write($"Documento de identidad ({patient.Document}): ");
            var document = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(document))
                patient.Document = document;

            Console.Write($"Edad ({patient.Age}): ");
            var ageInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ageInput) && int.TryParse(ageInput, out int age))
                patient.Age = age;

            Console.Write($"Teléfono ({patient.Phone}): ");
            var phone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(phone))
                patient.Phone = phone;

            Console.Write($"Correo electrónico ({patient.Email}): ");
            var email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
                patient.Email = email;

            var updatedPatient = await _patientService.UpdatePatientAsync(patientId, patient);
            Console.WriteLine($"\nPaciente actualizado exitosamente.");
            return updatedPatient;
        }

        public async Task<List<Patient>> SearchPatientsAsync()
        {
            Console.Clear();
            Console.WriteLine("BUSCAR PACIENTES");

            var searchTerm = GetSearchTermInput();
            var patients = await _patientService.SearchPatientsAsync(searchTerm);

            if (patients.Any())
            {
                Console.WriteLine($"\nPacientes encontrados:\n");
                foreach (var patient in patients)
                {
                    DisplayPatient(patient);
                }
            }
            else
            {
                Console.WriteLine("No se encontraron pacientes con ese criterio de búsqueda.");
            }
            return patients;
        }

        public async Task<List<Patient>> ListAllPatientsAsync()
        {
            Console.Clear();
            Console.WriteLine("LISTAR TODOS LOS PACIENTES");

            var patients = await _patientService.GetAllPatientsAsync();

            if (patients.Any())
            {
                Console.WriteLine($"\nTotal de pacientes: {patients.Count}\n");
                foreach (var patient in patients)
                {
                    DisplayPatient(patient);
                }
            }
            else
            {
                Console.WriteLine("No hay pacientes registrados.");
            }
            return patients;
        }

        public async Task<List<Appointment>> ShowPatientAppointmentsAsync()
        {
            Console.Clear();
            Console.WriteLine("CITAS DE PACIENTE");

            var patientId = GetIdInput("paciente");
            var patient = await _patientService.GetPatientByIdAsync(patientId);
            if (patient == null)
            {
                Console.WriteLine("Paciente no encontrado.");
                return new List<Appointment>();
            }

            var appointments = await _patientService.GetPatientAppointmentsAsync(patientId);

            Console.WriteLine($"\nCitas del paciente {patient.Name}:\n");

            if (appointments.Any())
            {
                foreach (var appointment in appointments)
                {
                    DisplayAppointment(appointment);
                }
            }
            else
            {
                Console.WriteLine("El paciente no tiene citas registradas.");
            }
            return appointments;
        }

        public async Task<bool> DeletePatientAsync()
        {
            Console.Clear();
            Console.WriteLine("ELIMINAR PACIENTE");

            var patientId = GetIdInput("paciente a eliminar");
            var patient = await _patientService.GetPatientByIdAsync(patientId);
            if (patient == null)
            {
                Console.WriteLine("Paciente no encontrado.");
                return false;
            }

            var confirmed = GetConfirmationInput($"¿Está seguro de que desea eliminar al paciente {patient.Name}?");
            if (confirmed)
            {
                var success = await _patientService.DeletePatientAsync(patientId);
                if (success)
                {
                    Console.WriteLine("Paciente eliminado exitosamente.");
                    return true;
                }
                else
                {
                    Console.WriteLine("No se pudo eliminar el paciente.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
                return false;
            }
        }

        #endregion

        #region Métodos de Doctor

        public async Task<Doctor> CreateDoctorAsync()
        {
            Console.Clear();
            Console.WriteLine("REGISTRAR NUEVO MÉDICO");

            var doctor = GetDoctorInput();
            var createdDoctor = await _doctorService.CreateDoctorAsync(doctor);
            Console.WriteLine($"\nMédico registrado exitosamente con ID: {createdDoctor.Id}");
            return createdDoctor;
        }

        public async Task<Doctor?> UpdateDoctorAsync()
        {
            Console.Clear();
            Console.WriteLine("EDITAR MÉDICO");

            var doctorId = GetIdInput("médico a editar");
            var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                Console.WriteLine("Médico no encontrado.");
                return null;
            }

            Console.WriteLine($"\nEditando médico: {doctor.Name}");
            Console.WriteLine("Deje en blanco para mantener el valor actual.\n");

            Console.Write($"Nombre completo ({doctor.Name}): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                doctor.Name = name;

            Console.Write($"Documento de identidad ({doctor.Document}): ");
            var document = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(document))
                doctor.Document = document;

            Console.Write($"Especialidad ({doctor.Specialty}): ");
            var specialty = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(specialty))
                doctor.Specialty = specialty;

            Console.Write($"Teléfono ({doctor.Phone}): ");
            var phone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(phone))
                doctor.Phone = phone;

            Console.Write($"Correo electrónico ({doctor.Email}): ");
            var email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
                doctor.Email = email;

            var updatedDoctor = await _doctorService.UpdateDoctorAsync(doctorId, doctor);
            Console.WriteLine($"\nMédico actualizado exitosamente.");
            return updatedDoctor;
        }

        public async Task<List<Doctor>> SearchDoctorsAsync()
        {
            Console.Clear();
            Console.WriteLine("BUSCAR MÉDICOS");

            var searchTerm = GetSearchTermInput();
            var doctors = await _doctorService.SearchDoctorsAsync(searchTerm);

            if (doctors.Any())
            {
                Console.WriteLine($"\nMédicos encontrados:\n");
                foreach (var doctor in doctors)
                {
                    DisplayDoctor(doctor);
                }
            }
            else
            {
                Console.WriteLine("No se encontraron médicos con ese criterio de búsqueda.");
            }
            return doctors;
        }

        public async Task<List<Doctor>> ListAllDoctorsAsync()
        {
            Console.Clear();
            Console.WriteLine("LISTAR TODOS LOS MÉDICOS");

            var doctors = await _doctorService.GetAllDoctorsAsync();

            if (doctors.Any())
            {
                Console.WriteLine($"\nTotal de médicos: {doctors.Count}\n");
                foreach (var doctor in doctors)
                {
                    DisplayDoctor(doctor);
                }
            }
            else
            {
                Console.WriteLine("No hay médicos registrados.");
            }
            return doctors;
        }

        public async Task<List<Doctor>> ListDoctorsBySpecialtyAsync()
        {
            Console.Clear();
            Console.WriteLine("MÉDICOS POR ESPECIALIDAD");

            var specialties = await _doctorService.GetAllSpecialtiesAsync();

            if (specialties.Any())
            {
                Console.WriteLine("\nEspecialidades disponibles:");
                for (int i = 0; i < specialties.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {specialties[i]}");
                }
                Console.WriteLine();
                Console.Write("Seleccione una especialidad: ");

                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= specialties.Count)
                {
                    var specialty = specialties[choice - 1];
                    var doctors = await _doctorService.GetDoctorsBySpecialtyAsync(specialty);

                    Console.WriteLine($"\nMédicos de la especialidad '{specialty}':\n");

                    if (doctors.Any())
                    {
                        foreach (var doctor in doctors)
                        {
                            DisplayDoctor(doctor);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No hay médicos registrados en la especialidad '{specialty}'.");
                    }
                    return doctors;
                }
                else
                {
                    Console.WriteLine("Opción inválida.");
                    return new List<Doctor>();
                }
            }
            else
            {
                Console.WriteLine("No hay especialidades registradas.");
                return new List<Doctor>();
            }
        }

        public async Task<List<Appointment>> ShowDoctorAppointmentsAsync()
        {
            Console.Clear();
            Console.WriteLine("CITAS DE MÉDICO");

            var doctorId = GetIdInput("médico");
            var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                Console.WriteLine("Médico no encontrado.");
                return new List<Appointment>();
            }

            var appointments = await _doctorService.GetDoctorAppointmentsAsync(doctorId);

            Console.WriteLine($"\nCitas del Dr. {doctor.Name}:\n");

            if (appointments.Any())
            {
                foreach (var appointment in appointments)
                {
                    DisplayAppointment(appointment);
                }
            }
            else
            {
                Console.WriteLine("El médico no tiene citas registradas.");
            }
            return appointments;
        }

        public async Task<bool> DeleteDoctorAsync()
        {
            Console.Clear();
            Console.WriteLine("ELIMINAR MÉDICO");

            var doctorId = GetIdInput("médico a eliminar");
            var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                Console.WriteLine("Médico no encontrado.");
                return false;
            }

            var confirmed = GetConfirmationInput($"¿Está seguro de que desea eliminar al médico {doctor.Name}?");
            if (confirmed)
            {
                var success = await _doctorService.DeleteDoctorAsync(doctorId);
                if (success)
                {
                    Console.WriteLine("Médico eliminado exitosamente.");
                    return true;
                }
                else
                {
                    Console.WriteLine("No se pudo eliminar el médico.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
                return false;
            }
        }

        #endregion

        #region Métodos de Citas

        public async Task<Appointment> CreateAppointmentAsync()
        {
            Console.Clear();
            Console.WriteLine("AGENDAR NUEVA CITA");

            var appointment = GetAppointmentInput();
            var createdAppointment = await _appointmentService.CreateAppointmentAsync(appointment);
            Console.WriteLine($"\nCita agendada exitosamente con ID: {createdAppointment.Id}");
            Console.WriteLine("Se ha enviado un correo de confirmación al paciente.");
            return createdAppointment;
        }

        public async Task<Appointment?> UpdateAppointmentAsync()
        {
            Console.Clear();
            Console.WriteLine("EDITAR CITA");

            var appointmentId = GetIdInput("cita a editar");
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null)
            {
                Console.WriteLine("Cita no encontrada.");
                return null;
            }

            Console.WriteLine($"\nEditando cita ID: {appointment.Id}");
            Console.WriteLine("Deje en blanco para mantener el valor actual.\n");

            Console.Write($"ID del paciente ({appointment.PatientId}): ");
            var patientIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(patientIdInput) && int.TryParse(patientIdInput, out int patientId))
                appointment.PatientId = patientId;

            Console.Write($"ID del médico ({appointment.DoctorId}): ");
            var doctorIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(doctorIdInput) && int.TryParse(doctorIdInput, out int doctorId))
                appointment.DoctorId = doctorId;

            Console.Write($"Fecha ({appointment.AppointmentDate:dd/MM/yyyy}): ");
            var dateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dateInput) && ValidationHelper.TryParseDate(dateInput, out DateTime date))
                appointment.AppointmentDate = date;

            Console.Write($"Hora ({appointment.AppointmentTime:hh\\:mm}): ");
            var timeInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(timeInput) && ValidationHelper.TryParseTime(timeInput, out TimeSpan time))
                appointment.AppointmentTime = time;

            Console.Write($"Notas del doctor ({appointment.Notes ?? "ninguna"}): ");
            var notes = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(notes))
                appointment.Notes = notes;

            var updatedAppointment = await _appointmentService.UpdateAppointmentAsync(appointmentId, appointment);
            Console.WriteLine($"\nCita actualizada exitosamente.");
            return updatedAppointment;
        }

        public async Task<bool> CancelAppointmentAsync()
        {
            Console.Clear();
            Console.WriteLine("CANCELAR CITA");

            var appointmentId = GetIdInput("cita a cancelar");
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null)
            {
                Console.WriteLine("Cita no encontrada.");
                return false;
            }

            Console.WriteLine($"\nCita a cancelar:");
            Console.WriteLine($"Paciente: {appointment.Patient.Name}");
            Console.WriteLine($"Médico: Dr. {appointment.Doctor.Name}");
            Console.WriteLine($"Fecha: {appointment.AppointmentDate:dd/MM/yyyy}");
            Console.WriteLine($"Hora: {appointment.AppointmentTime:hh\\:mm}");

            var confirmed = GetConfirmationInput("¿Está seguro de que desea cancelar esta cita?");
            if (confirmed)
            {
                var success = await _appointmentService.CancelAppointmentAsync(appointmentId);
                if (success)
                {
                    Console.WriteLine("Cita cancelada exitosamente.");
                    return true;
                }
                else
                {
                    Console.WriteLine("No se pudo cancelar la cita.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
                return false;
            }
        }

        public async Task<bool> MarkAppointmentAsAttendedAsync()
        {
            Console.Clear();
            Console.WriteLine("MARCAR CITA COMO ATENDIDA");

            var appointmentId = GetIdInput("cita a marcar como atendida");
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null)
            {
                Console.WriteLine("Cita no encontrada.");
                return false;
            }

            Console.WriteLine($"\nCita a marcar como atendida:");
            Console.WriteLine($"Paciente: {appointment.Patient.Name}");
            Console.WriteLine($"Médico: Dr. {appointment.Doctor.Name}");
            Console.WriteLine($"Fecha: {appointment.AppointmentDate:dd/MM/yyyy}");
            Console.WriteLine($"Hora: {appointment.AppointmentTime:hh\\:mm}");

            var confirmed = GetConfirmationInput("¿Está seguro de que desea marcar esta cita como atendida?");
            if (confirmed)
            {
                var success = await _appointmentService.MarkAppointmentAsAttendedAsync(appointmentId);
                if (success)
                {
                    Console.WriteLine("Cita marcada como atendida exitosamente.");
                    return true;
                }
                else
                {
                    Console.WriteLine("No se pudo marcar la cita como atendida.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
                return false;
            }
        }

        public async Task<List<Appointment>> ListAllAppointmentsAsync()
        {
            Console.Clear();
            Console.WriteLine("LISTAR TODAS LAS CITAS");

            var appointments = await _appointmentService.GetAllAppointmentsAsync();

            if (appointments.Any())
            {
                Console.WriteLine($"\nTotal de citas: {appointments.Count}\n");
                foreach (var appointment in appointments)
                {
                    DisplayAppointment(appointment);
                }
            }
            else
            {
                Console.WriteLine("No hay citas registradas.");
            }
            return appointments;
        }

        public async Task<List<Appointment>> ListAppointmentsByDateAsync()
        {
            Console.Clear();
            Console.WriteLine("CITAS POR FECHA");

            var date = GetDateInput();
            var appointments = await _appointmentService.GetAppointmentsByDateAsync(date);

            Console.WriteLine($"\nCitas para el {date:dd/MM/yyyy}:\n");

            if (appointments.Any())
            {
                foreach (var appointment in appointments)
                {
                    DisplayAppointment(appointment);
                }
            }
            else
            {
                Console.WriteLine($"No hay citas programadas para el {date:dd/MM/yyyy}.");
            }
            return appointments;
        }

        public async Task<List<Appointment>> ListAppointmentsByStatusAsync()
        {
            Console.Clear();
            Console.WriteLine("CITAS POR ESTADO");

            Console.WriteLine("1. Programadas");
            Console.WriteLine("2. Atendidas");
            Console.WriteLine("3. Canceladas");
            Console.WriteLine();
            Console.Write("Seleccione un estado: ");

            var choice = Console.ReadLine();
            AppointmentStatus status = choice switch
            {
                "1" => AppointmentStatus.Scheduled,
                "2" => AppointmentStatus.Attended,
                "3" => AppointmentStatus.Cancelled,
                _ => throw new ArgumentException("Opción inválida")
            };

            var appointments = await _appointmentService.GetAppointmentsByStatusAsync(status);
            var statusText = GetStatusInSpanish(status);

            Console.WriteLine($"\nCitas con estado '{statusText}':\n");

            if (appointments.Any())
            {
                foreach (var appointment in appointments)
                {
                    DisplayAppointment(appointment);
                }
            }
            else
            {
                Console.WriteLine($"No hay citas con estado '{statusText}'.");
            }
            return appointments;
        }

        public async Task<bool> DeleteAppointmentAsync()
        {
            Console.Clear();
            Console.WriteLine("ELIMINAR CITA");

            var appointmentId = GetIdInput("cita a eliminar");
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null)
            {
                Console.WriteLine("Cita no encontrada.");
                return false;
            }

            Console.WriteLine($"\nCita a eliminar:");
            Console.WriteLine($"Paciente: {appointment.Patient.Name}");
            Console.WriteLine($"Médico: Dr. {appointment.Doctor.Name}");
            Console.WriteLine($"Fecha: {appointment.AppointmentDate:dd/MM/yyyy}");
            Console.WriteLine($"Hora: {appointment.AppointmentTime:hh\\:mm}");

            var confirmed = GetConfirmationInput("¿Está seguro de que desea eliminar esta cita?");
            if (confirmed)
            {
                var success = await _appointmentService.DeleteAppointmentAsync(appointmentId);
                if (success)
                {
                    Console.WriteLine("Cita eliminada exitosamente.");
                    return true;
                }
                else
                {
                    Console.WriteLine("No se pudo eliminar la cita.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
                return false;
            }
        }

        #endregion

        #region Métodos de Email

        public async Task<List<EmailLog>> ShowAllEmailHistoryAsync()
        {
            Console.Clear();
            Console.WriteLine("HISTORIAL COMPLETO DE CORREOS");

            var emailLogs = await _emailService.GetEmailHistoryAsync();

            if (emailLogs.Any())
            {
                Console.WriteLine($"\nTotal de correos: {emailLogs.Count}\n");
                foreach (var emailLog in emailLogs)
                {
                    DisplayEmailLog(emailLog);
                }
            }
            else
            {
                Console.WriteLine("No hay correos en el historial.");
            }
            return emailLogs;
        }

        public async Task<List<EmailLog>> ShowEmailHistoryByStatusAsync(EmailStatus status)
        {
            Console.Clear();
            Console.WriteLine($"CORREOS CON ESTADO: {GetEmailStatusInSpanish(status)}");

            var emailLogs = await _emailService.GetEmailHistoryByStatusAsync(status);

            if (emailLogs.Any())
            {
                Console.WriteLine($"\nTotal de correos: {emailLogs.Count}\n");
                foreach (var emailLog in emailLogs)
                {
                    DisplayEmailLog(emailLog);
                }
            }
            else
            {
                Console.WriteLine($"No hay correos con estado '{GetEmailStatusInSpanish(status)}'.");
            }
            return emailLogs;
        }

        public async Task<bool> TestEmailConfigurationAsync()
        {
            Console.Clear();
            Console.WriteLine("PROBAR CONFIGURACIÓN DE EMAIL");

            Console.Write("Ingrese un email de prueba: ");
            var testEmail = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(testEmail))
            {
                Console.WriteLine("Email no válido.");
                return false;
            }

            Console.WriteLine("\nEnviando correo de prueba...");
            
            try
            {
                var success = await _emailService.TestEmailConfigurationAsync(testEmail);
                
                if (success)
                {
                    Console.WriteLine("Correo de prueba enviado exitosamente.");
                    return true;
                }
                else
                {
                    Console.WriteLine("No se pudo enviar el correo de prueba.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ErrorHandler.HandleEmailError(ex);
                Console.WriteLine(errorMessage);
                return false;
            }
        }

        #endregion

        #region Métodos de Estadísticas

        public async Task ShowStatisticsAsync()
        {
            Console.Clear();
            Console.WriteLine("ESTADÍSTICAS DEL SISTEMA");

            try
            {
                var stats = await _appointmentService.GetAppointmentStatisticsAsync();
                var totalPatients = await _patientService.GetAllPatientsAsync();
                var totalDoctors = await _doctorService.GetAllDoctorsAsync();
                var emailLogs = await _emailService.GetEmailHistoryAsync();

                Console.WriteLine("\n=== RESUMEN GENERAL ===");
                Console.WriteLine($"Total de pacientes: {totalPatients.Count}");
                Console.WriteLine($"Total de médicos: {totalDoctors.Count}");
                Console.WriteLine($"Total de citas: {stats["Total"]}");
                Console.WriteLine($"Total de correos: {emailLogs.Count}");

                Console.WriteLine("\n=== ESTADÍSTICAS DE CITAS ===");
                Console.WriteLine($"Citas programadas: {stats["Programadas"]}");
                Console.WriteLine($"Citas atendidas: {stats["Atendidas"]}");
                Console.WriteLine($"Citas canceladas: {stats["Canceladas"]}");

                Console.WriteLine("\n=== ESTADÍSTICAS DE CORREOS ===");
                var sentEmails = emailLogs.Count(e => e.Status == EmailStatus.Sent);
                var failedEmails = emailLogs.Count(e => e.Status == EmailStatus.Failed);
                var notSentEmails = emailLogs.Count(e => e.Status == EmailStatus.NotSent);

                Console.WriteLine($"Correos enviados: {sentEmails}");
                Console.WriteLine($"Correos no enviados: {notSentEmails}");
                Console.WriteLine($"Correos fallidos: {failedEmails}");

                if (emailLogs.Count > 0)
                {
                    var successRate = (double)sentEmails / emailLogs.Count * 100;
                    Console.WriteLine($"Tasa de éxito: {successRate:F1}%");
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ErrorHandler.HandleError(ex, "obtención de estadísticas");
                Console.WriteLine(errorMessage);
            }
        }

        #endregion

        #region Métodos Auxiliares

        private string FormatDateSafely(DateTime date)
        {
            try
            {
                return date.ToString("dd/MM/yyyy");
            }
            catch
            {
                return "Fecha no disponible";
            }
        }

        private string FormatTimeSafely(TimeSpan time)
        {
            try
            {
                if (time == TimeSpan.Zero || time.TotalHours >= 24 || time.TotalHours < 0)
                {
                    return "Hora no disponible";
                }
                return time.ToString(@"hh\:mm");
            }
            catch
            {
                return "Hora no disponible";
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

        private string GetEmailStatusInSpanish(EmailStatus status)
        {
            return status switch
            {
                EmailStatus.Sent => "Enviado",
                EmailStatus.NotSent => "No Enviado",
                EmailStatus.Failed => "Fallido",
                _ => "Desconocido"
            };
        }

        #endregion
    }
}
