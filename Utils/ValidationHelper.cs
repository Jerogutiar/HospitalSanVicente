using System.Text.RegularExpressions;

namespace HospitalSanVicente.Utils
{
    public static class ValidationHelper
    {
        // Validaciones de entrada básicas
        public static bool IsValidString(string input, int minLength = 1, int maxLength = 100, bool allowNumbers = true)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            if (input.Length < minLength || input.Length > maxLength)
                return false;

            // Verificar caracteres especiales peligrosos
            if (input.Contains("<") || input.Contains(">") || input.Contains("'") || 
                input.Contains("\"") || input.Contains(";") || input.Contains("--") ||
                input.Contains("/*") || input.Contains("*/") || input.Contains("xp_"))
                return false;

            // Si no se permiten números, verificar que no contenga dígitos
            if (!allowNumbers && input.Any(char.IsDigit))
                return false;

            return true;
        }

        public static bool IsValidName(string name)
        {
            if (!IsValidString(name, 2, 100, false))
                return false;

            // Verificar que contenga solo letras, espacios y algunos caracteres especiales permitidos
            var namePattern = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s\-\.]+$";
            return Regex.IsMatch(name, namePattern);
        }

        public static bool IsValidDocument(string document)
        {
            if (string.IsNullOrWhiteSpace(document))
                return false;

            // Longitud entre 5 y 20 caracteres
            if (document.Length < 5 || document.Length > 20)
                return false;

            // Solo números
            return document.All(char.IsDigit);
        }

        public static bool IsValidAge(int age)
        {
            return age >= 1 && age <= 150;
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Longitud entre 7 y 20 caracteres
            if (phone.Length < 7 || phone.Length > 20)
                return false;

            // Solo números, espacios, guiones, paréntesis y el símbolo +
            var phonePattern = @"^[\d\s\-\(\)\+]+$";
            return Regex.IsMatch(phone, phonePattern);
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            if (email.Length > 100)
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidSpecialty(string specialty)
        {
            if (!IsValidString(specialty, 2, 50, false))
                return false;

            // Verificar que contenga solo letras, espacios y algunos caracteres especiales
            var specialtyPattern = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s\-\.]+$";
            return Regex.IsMatch(specialty, specialtyPattern);
        }

        public static bool IsValidDate(DateTime date)
        {
            // No fechas en el pasado (excepto hoy)
            if (date.Date < DateTime.Today)
                return false;

            // No fechas muy futuras (máximo 2 años)
            if (date.Date > DateTime.Today.AddYears(2))
                return false;

            return true;
        }

        public static bool IsValidTime(TimeSpan time)
        {
            // Horario de trabajo: 6:00 AM a 10:00 PM
            var startTime = new TimeSpan(6, 0, 0);
            var endTime = new TimeSpan(22, 0, 0);
            
            return time >= startTime && time <= endTime;
        }

        public static bool IsValidNotes(string notes)
        {
            if (string.IsNullOrWhiteSpace(notes))
                return true; // Las notas son opcionales

            if (notes.Length > 500)
                return false;

            return true;
        }

        public static bool IsValidId(int id)
        {
            return id > 0 && id <= int.MaxValue;
        }

        public static bool IsValidMenuChoice(string choice, int maxOptions)
        {
            if (string.IsNullOrWhiteSpace(choice))
                return false;

            if (!int.TryParse(choice, out int choiceInt))
                return false;

            return choiceInt >= 0 && choiceInt <= maxOptions;
        }



        // Validación de formato de fecha
        public static bool TryParseDate(string dateString, out DateTime date)
        {
            date = DateTime.MinValue;
            
            if (string.IsNullOrWhiteSpace(dateString))
                return false;

            // Limpiar la entrada
            dateString = dateString.Trim();

            // Solo permitir el formato dd/MM/yyyy
            if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, 
                System.Globalization.DateTimeStyles.None, out date))
            {
                return IsValidDate(date);
            }

            return false;
        }

        // Validación de formato de hora
        public static bool TryParseTime(string timeString, out TimeSpan time)
        {
            time = TimeSpan.Zero;
            
            if (string.IsNullOrWhiteSpace(timeString))
                return false;

            // Limpiar la entrada
            timeString = timeString.Trim();

            // Solo permitir formato de 24 horas (hh:mm)
            var formats = new[] { "hh\\:mm", "h\\:mm", "HH\\:mm", "H\\:mm" };
            
            foreach (var format in formats)
            {
                if (TimeSpan.TryParseExact(timeString, format, null, out time))
                {
                    return IsValidTime(time);
                }
            }

            // Intentar parseo directo como último recurso
            if (TimeSpan.TryParse(timeString, out time))
            {
                return IsValidTime(time);
            }

            return false;
        }
    }
}
