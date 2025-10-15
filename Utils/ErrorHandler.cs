using System;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalSanVicente.Utils
{
    public static class ErrorHandler
    {
        /// <summary>
        /// Maneja errores específicos y los traduce al español
        /// </summary>
        public static string HandleError(Exception ex, string operation)
        {
            return ex switch
            {
                ArgumentNullException => $"Error de validación: {ex.Message}",
                ArgumentException => $"Error de argumento: {ex.Message}",
                InvalidOperationException => $"Error de operación: {ex.Message}",
                DbUpdateException dbEx => HandleDatabaseError(dbEx, operation),
                FormatException => "Error de formato: Los datos ingresados no tienen el formato correcto.",
                OverflowException => "Error de rango: El valor ingresado es demasiado grande o pequeño.",
                TimeoutException => "Error de tiempo: La operación tardó demasiado tiempo en completarse.",
                UnauthorizedAccessException => "Error de acceso: No tiene permisos para realizar esta operación.",
                _ => HandleGenericError(ex, operation)
            };
        }

        /// <summary>
        /// Maneja errores específicos de base de datos
        /// </summary>
        private static string HandleDatabaseError(DbUpdateException dbEx, string operation)
        {
            var innerException = dbEx.InnerException;
            
            return innerException switch
            {
                MySqlConnector.MySqlException mysqlEx => HandleMySqlError(mysqlEx, operation),
                _ => $"Error de base de datos durante {operation}: {innerException?.Message ?? dbEx.Message}"
            };
        }

        /// <summary>
        /// Maneja errores específicos de MySQL
        /// </summary>
        private static string HandleMySqlError(MySqlConnector.MySqlException mysqlEx, string operation)
        {
            return mysqlEx.Number switch
            {
                1062 => "Error: Ya existe un registro con los mismos datos únicos (documento o email duplicado).",
                1451 => "Error: No se puede eliminar este registro porque está siendo utilizado por otros datos.",
                1048 => "Error: Faltan datos requeridos para completar la operación.",
                1054 => "Error: Campo de base de datos no encontrado.",
                1146 => "Error: Tabla de base de datos no encontrada.",
                2006 => "Error: Conexión con la base de datos perdida. Intente nuevamente.",
                2013 => "Error: Conexión con la base de datos perdida durante la consulta.",
                _ => $"Error de base de datos durante {operation}: {mysqlEx.Message}"
            };
        }

        /// <summary>
        /// Maneja errores genéricos y traduce mensajes comunes
        /// </summary>
        private static string HandleGenericError(Exception ex, string operation)
        {
            var message = ex.Message;
            
            // Traducir mensajes comunes de .NET
            var translatedMessage = message switch
            {
                "Input string was not in a correct format." => "El formato de entrada no es correcto. Verifique que la fecha esté en formato dd/mm/yyyy y la hora en formato hh:mm.",
                "The input string was not in a correct format." => "El formato de entrada no es correcto. Verifique que la fecha esté en formato dd/mm/yyyy y la hora en formato hh:mm.",
                "String was not recognized as a valid DateTime." => "La fecha ingresada no es válida. Use el formato dd/mm/yyyy.",
                "String was not recognized as a valid TimeSpan." => "La hora ingresada no es válida. Use el formato hh:mm (ejemplo: 10:30, 14:15).",
                "Value was either too large or too small for an Int32." => "El valor ingresado es demasiado grande o pequeño para un número entero.",
                "Object reference not set to an instance of an object." => "Error interno: Referencia de objeto no establecida.",
                "Index was outside the bounds of the array." => "Error interno: Índice fuera de los límites del arreglo.",
                _ => message
            };

            return $"Error durante {operation}: {translatedMessage}";
        }

        /// <summary>
        /// Maneja errores específicos de validación
        /// </summary>
        public static string HandleValidationError(string field, string error)
        {
            return $"Error de validación en {field}: {error}";
        }

        /// <summary>
        /// Maneja errores específicos de operaciones de citas
        /// </summary>
        public static string HandleAppointmentError(Exception ex)
        {
            return ex switch
            {
                ArgumentException => ex.Message,
                InvalidOperationException => ex.Message,
                _ => HandleError(ex, "gestión de citas")
            };
        }

        /// <summary>
        /// Maneja errores específicos de operaciones de pacientes
        /// </summary>
        public static string HandlePatientError(Exception ex)
        {
            return ex switch
            {
                ArgumentException => ex.Message,
                InvalidOperationException => ex.Message,
                _ => HandleError(ex, "gestión de pacientes")
            };
        }

        /// <summary>
        /// Maneja errores específicos de operaciones de médicos
        /// </summary>
        public static string HandleDoctorError(Exception ex)
        {
            return ex switch
            {
                ArgumentException => ex.Message,
                InvalidOperationException => ex.Message,
                _ => HandleError(ex, "gestión de médicos")
            };
        }

        /// <summary>
        /// Maneja errores específicos de operaciones de correo
        /// </summary>
        public static string HandleEmailError(Exception ex)
        {
            return ex switch
            {
                System.Net.Mail.SmtpException => "Error de servidor de correo: No se pudo enviar el mensaje.",
                System.Net.Sockets.SocketException => "Error de conexión: No se pudo conectar al servidor de correo.",
                System.Security.Authentication.AuthenticationException => "Error de autenticación: Credenciales de correo inválidas.",
                TimeoutException => "Error de tiempo: El servidor de correo tardó demasiado en responder.",
                _ => HandleError(ex, "envío de correo")
            };
        }
    }
}
