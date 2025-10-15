using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HospitalSanVicente.Data;
using HospitalSanVicente.Services;
using HospitalSanVicente.Models;
using HospitalSanVicente.Utils;

namespace HospitalSanVicente
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configurar servicios
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            // Crear y migrar la base de datos
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<HospitalDbContext>();
                await context.Database.EnsureCreatedAsync();
            }

            // Iniciar la aplicación
            var app = serviceProvider.GetRequiredService<HospitalApp>();
            await app.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Configurar Entity Framework con MySQL
            var connectionString = "Server=168.119.183.3;Port=3307;Database=PruebaDeDesempeñoJeronimo;Uid=root;Pwd=g0tIFJEQsKHm5$34Pxu1;";
            services.AddDbContext<HospitalDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Registrar servicios con sus interfaces
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IHospitalUIService, HospitalUIService>();

            // Registrar aplicación principal
            services.AddScoped<HospitalApp>();
        }
    }

    public class HospitalApp
    {
        private readonly IHospitalUIService _uiService;

        public HospitalApp(IHospitalUIService uiService)
        {
            _uiService = uiService;
        }

        public async Task RunAsync()
        {
            Console.Clear();
            Console.WriteLine("    SISTEMA DE GESTIÓN HOSPITAL SAN VICENTE   ");
            Console.WriteLine();

            while (true)
            {
                try
                {
                    _uiService.ShowMainMenu();
                    var choice = Console.ReadLine();

                    // Validar entrada del menú
                    if (string.IsNullOrWhiteSpace(choice) || !ValidationHelper.IsValidMenuChoice(choice, 5))
                    {
                        Console.WriteLine("Opción no válida. Por favor, seleccione una opción del menú (0-5).");
                        continue;
                    }

                    switch (choice)
                    {
                        case "1":
                            await PatientManagementMenu();
                            break;
                        case "2":
                            await DoctorManagementMenu();
                            break;
                        case "3":
                            await AppointmentManagementMenu();
                            break;
                        case "4":
                            await EmailHistoryMenu();
                            break;
                        case "5":
                            await _uiService.ShowStatisticsAsync();
                            break;
                        case "0":
                            Console.WriteLine("¡Gracias por usar el sistema! ¡Hasta luego!");
                            return;
                        default:
                            Console.WriteLine("Opción no válida. Por favor, seleccione una opción del menú.");
                            break;
                    }

                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadLine();
                    Console.Clear();
                }
                catch (Exception ex)
                {
                    var errorMessage = ErrorHandler.HandleError(ex, "operación del menú principal");
                    Console.WriteLine(errorMessage);
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        private async Task PatientManagementMenu()
        {
            while (true)
            {
                _uiService.ShowPatientManagementMenu();
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await _uiService.CreatePatientAsync();
                            break;
                        case "2":
                            await _uiService.UpdatePatientAsync();
                            break;
                        case "3":
                            await _uiService.SearchPatientsAsync();
                            break;
                        case "4":
                            await _uiService.ListAllPatientsAsync();
                            break;
                        case "5":
                            await _uiService.ShowPatientAppointmentsAsync();
                            break;
                        case "6":
                            await _uiService.DeletePatientAsync();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Opción no válida.");
                            break;
                    }

                    if (choice != "0")
                    {
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = ErrorHandler.HandlePatientError(ex);
                    Console.WriteLine(errorMessage);
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadLine();
                }
            }
        }

        private async Task DoctorManagementMenu()
        {
            while (true)
            {
                _uiService.ShowDoctorManagementMenu();
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await _uiService.CreateDoctorAsync();
                            break;
                        case "2":
                            await _uiService.UpdateDoctorAsync();
                            break;
                        case "3":
                            await _uiService.SearchDoctorsAsync();
                            break;
                        case "4":
                            await _uiService.ListAllDoctorsAsync();
                            break;
                        case "5":
                            await _uiService.ListDoctorsBySpecialtyAsync();
                            break;
                        case "6":
                            await _uiService.ShowDoctorAppointmentsAsync();
                            break;
                        case "7":
                            await _uiService.DeleteDoctorAsync();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Opción no válida.");
                            break;
                    }

                    if (choice != "0")
                    {
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = ErrorHandler.HandleDoctorError(ex);
                    Console.WriteLine(errorMessage);
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadLine();
                }
            }
        }

        private async Task AppointmentManagementMenu()
        {
            while (true)
            {
                _uiService.ShowAppointmentManagementMenu();
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await _uiService.CreateAppointmentAsync();
                            break;
                        case "2":
                            await _uiService.UpdateAppointmentAsync();
                            break;
                        case "3":
                            await _uiService.CancelAppointmentAsync();
                            break;
                        case "4":
                            await _uiService.MarkAppointmentAsAttendedAsync();
                            break;
                        case "5":
                            await _uiService.ListAllAppointmentsAsync();
                            break;
                        case "6":
                            await _uiService.ListAppointmentsByDateAsync();
                            break;
                        case "7":
                            await _uiService.ListAppointmentsByStatusAsync();
                            break;
                        case "8":
                            await _uiService.ShowPatientAppointmentsAsync();
                            break;
                        case "9":
                            await _uiService.ShowDoctorAppointmentsAsync();
                            break;
                        case "10":
                            await _uiService.DeleteAppointmentAsync();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Opción no válida.");
                            break;
                    }

                    if (choice != "0")
                    {
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = ErrorHandler.HandleAppointmentError(ex);
                    Console.WriteLine(errorMessage);
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadLine();
                }
            }
        }

        private async Task EmailHistoryMenu()
        {
            while (true)
            {
                _uiService.ShowEmailHistoryMenu();
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await _uiService.ShowAllEmailHistoryAsync();
                            break;
                        case "2":
                            await _uiService.ShowEmailHistoryByStatusAsync(EmailStatus.Sent);
                            break;
                        case "3":
                            await _uiService.ShowEmailHistoryByStatusAsync(EmailStatus.NotSent);
                            break;
                        case "4":
                            await _uiService.ShowEmailHistoryByStatusAsync(EmailStatus.Failed);
                            break;
                        case "5":
                            await _uiService.TestEmailConfigurationAsync();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Opción no válida.");
                            break;
                    }

                    if (choice != "0")
                    {
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = ErrorHandler.HandleEmailError(ex);
                    Console.WriteLine(errorMessage);
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadLine();
                }
            }
        }
    }
}