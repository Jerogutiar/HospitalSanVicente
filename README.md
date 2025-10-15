# Sistema de Gestión Hospital San Vicente

## Descripción

Sistema de consola desarrollado en C# para la gestión digital de citas médicas, pacientes y médicos del Hospital San Vicente. Elimina la dependencia de registros manuales y automatiza la programación de citas con validaciones de conflictos.

## Características Principales

### Gestión de Pacientes
- Registro, edición, búsqueda y eliminación de pacientes
- Validación de documentos únicos
- Historial de citas por paciente

### Gestión de Médicos
- Registro, edición, búsqueda y eliminación de médicos
- Filtrado por especialidad
- Historial de citas por médico

### Gestión de Citas
- Programación con validación de conflictos de horarios
- Cancelación y marcado como atendidas
- Listado por fecha, estado, paciente o médico
- Envío automático de correos de confirmación

### Sistema de Notificaciones
- Envío automático de correos usando Gmail SMTP
- Historial completo de envíos con estados
- Formato profesional con información completa

### Estadísticas
- Contadores de pacientes, médicos y citas
- Estadísticas por estado de citas
- Métricas de correos electrónicos

## Tecnologías

- **.NET 8.0** - Framework de desarrollo
- **Entity Framework Core** - ORM para acceso a datos
- **MySQL** - Base de datos relacional remota
- **MailKit** - Envío de correos electrónicos
- **Dependency Injection** - Inyección de dependencias

## Instalación

### Requisitos
- .NET 8.0 Runtime
- Acceso a internet (para base de datos MySQL)

### Pasos
1. Clonar el repositorio
2. Restaurar dependencias: `dotnet restore`
3. Compilar: `dotnet build`
4. Ejecutar: `dotnet run`

### Configuración de Base de Datos
El sistema se conecta automáticamente a una base de datos MySQL remota:
- **Host:** 168.119.183.3:3307
- **Base de datos:** PruebaDeDesempeñoJeronimo
- Las tablas se crean automáticamente

### Configuración de Correo
Configuración en `appsettings.json`:
```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "hospitalsanvicenteriwi@gmail.com",
    "SmtpPassword": "qjws fxmc zxcd bdnm",
    "FromName": "Hospital San Vicente",
    "FromEmail": "hospitalsanvicenteriwi@gmail.com"
  }
}
```

## Estructura del Proyecto

```
HospitalSanVicente/
├── Data/
│   └── HospitalDbContext.cs          # Contexto de Entity Framework
├── Models/
│   ├── Patient.cs                    # Modelo de Paciente
│   ├── Doctor.cs                     # Modelo de Médico
│   ├── Appointment.cs                # Modelo de Cita Médica
│   └── EmailLog.cs                   # Modelo de Log de Correos
├── Services/
│   ├── IPatientService.cs            # Interfaz para servicio de pacientes
│   ├── IDoctorService.cs             # Interfaz para servicio de médicos
│   ├── IAppointmentService.cs        # Interfaz para servicio de citas
│   ├── IEmailService.cs              # Interfaz para servicio de correos
│   ├── IHospitalUIService.cs         # Interfaz para servicio de UI
│   ├── PatientService.cs             # Implementación de lógica de negocio
│   ├── DoctorService.cs              # Implementación de lógica de negocio
│   ├── AppointmentService.cs         # Implementación de lógica de negocio
│   ├── EmailService.cs               # Implementación de lógica de negocio
│   └── HospitalUIService.cs          # Implementación de interfaz de usuario
├── Utils/
│   ├── ValidationHelper.cs           # Utilidades de validación
│   └── ErrorHandler.cs               # Manejo centralizado de errores
├── diagrams/
│   ├── Diagrama_Clases_Hospital.xml  # Archivo XML del diagrama de clases
│   ├── Diagrama_Casos_Uso_Hospital.xml # Archivo XML del diagrama de casos de uso
│   ├── Diagrama_Clases_Hospital.drawio.png # Imagen del diagrama de clases
│   └── Diagrama_Casos_Uso_Hospital.drawio.png # Imagen del diagrama de casos de uso
├── media/
│   ├── correo_confirmacion_cita.png  # Captura del correo de confirmación
│   └── demo_sistema_completo.mp4     # Video demostrativo del sistema
├── appsettings.json                  # Configuración de la aplicación
├── Program.cs                        # Punto de entrada y flujo principal
├── HospitalSanVicente.csproj         # Archivo de proyecto
└── README.md                         # Documentación del proyecto
```

## Uso del Sistema

### Menú Principal
1. **Gestión de Pacientes** - Administrar información de pacientes
2. **Gestión de Médicos** - Administrar información de médicos
3. **Gestión de Citas Médicas** - Programar y administrar citas
4. **Historial de Correos Electrónicos** - Ver logs de correos enviados
5. **Estadísticas del Sistema** - Ver resumen general del sistema
0. **Salir** - Cerrar la aplicación

### Operaciones Principales

#### Gestión de Pacientes
- **Registrar**: Nombre, documento único, edad, teléfono, email
- **Editar**: Modificar campos existentes
- **Buscar**: Por nombre, documento o email
- **Eliminar**: Con validación de citas futuras

#### Gestión de Médicos
- **Registrar**: Nombre, documento único, especialidad, teléfono, email
- **Editar**: Modificar información médica
- **Buscar**: Por nombre, documento o especialidad
- **Eliminar**: Con validación de citas futuras

#### Gestión de Citas
- **Agendar**: ID paciente, ID médico, fecha, hora, notas opcionales
- **Cancelar**: Cambiar estado a "Cancelada"
- **Marcar atendida**: Cambiar estado a "Atendida"
- **Listar**: Por fecha, estado, paciente o médico

## Validaciones y Reglas de Negocio

### Validaciones Generales
- Documentos únicos en el sistema
- Edad entre 1 y 150 años
- Formato de email válido
- Fecha y hora en formato correcto
- No citas en fechas pasadas

### Validaciones de Citas
- Un médico no puede tener dos citas en el mismo horario
- Un paciente no puede tener dos citas en el mismo horario
- Paciente y médico deben existir en el sistema
- No eliminar citas futuras sin cancelarlas primero

## Arquitectura

### Patrones de Diseño
- **Repository Pattern**: Servicios encapsulan lógica de acceso a datos
- **Dependency Injection**: Inyección de dependencias para servicios
- **Clean Architecture**: Separación clara de responsabilidades
- **Separation of Concerns**: UI separada de lógica de negocio

### Capas
1. **Presentación**: Interfaz de consola (Program.cs + HospitalUIService.cs)
2. **Lógica de Negocio**: Servicios (Services/)
3. **Acceso a Datos**: Entity Framework (Data/)
4. **Modelos**: Entidades de dominio (Models/)
5. **Utilidades**: Validaciones y manejo de errores (Utils/)

### POO Avanzada
- **Interfaces**: IPatientService, IDoctorService, IAppointmentService, IEmailService, IHospitalUIService
- **Abstracción**: Contratos bien definidos para extensibilidad
- **Inyección de Dependencias**: Configuración centralizada en Program.cs

## Diagramas UML

### Diagrama de Clases
![Diagrama de Clases](diagrams/Diagrama_Clases_Hospital.drawio.png)

Muestra la estructura completa del sistema con modelos, interfaces, implementaciones y relaciones.

### Diagrama de Casos de Uso
![Diagrama de Casos de Uso](diagrams/Diagrama_Casos_Uso_Hospital.drawio.png)

Ilustra las interacciones entre actores (Administrador, Recepcionista, Médico, Paciente) y funcionalidades del sistema.

## Demostración del Sistema

### Correo Electrónico de Confirmación
![Correo de Confirmación de Cita](media/correo_confirmacion_cita.png)

El sistema envía automáticamente correos profesionales con información completa de la cita.

### Video Demostrativo
[Ver Video Demostrativo Completo](media/demo_sistema_completo.mp4)

Video que muestra todas las funcionalidades del sistema funcionando en consola.

## Base de Datos

### Esquema de Tablas

#### Patients
- `Id` (PK), `Name`, `Document` (único), `Age`, `Phone`, `Email`, `CreatedAt`

#### Doctors
- `Id` (PK), `Name`, `Document` (único), `Specialty`, `Phone`, `Email`, `CreatedAt`

#### Appointments
- `Id` (PK), `PatientId` (FK), `DoctorId` (FK), `AppointmentDate`, `AppointmentTime`, `Status`, `Notes`, `CreatedAt`

#### EmailLogs
- `Id` (PK), `AppointmentId` (FK), `RecipientEmail`, `Subject`, `Body`, `Status`, `ErrorMessage`, `SentAt`

## Comandos Útiles

### Desarrollo
```bash
dotnet restore    # Restaurar paquetes
dotnet build      # Compilar proyecto
dotnet run        # Ejecutar aplicación
dotnet clean      # Limpiar proyecto
```

### Base de Datos
```bash
dotnet tool install --global dotnet-ef  # Instalar herramientas EF Core
dotnet ef migrations add NombreMigracion  # Crear migración
dotnet ef database update  # Aplicar migraciones
```

## Estado del Proyecto

### Funcionalidades Completadas
- Gestión completa de pacientes, médicos y citas
- Sistema de correos funcional con Gmail SMTP
- Validaciones robustas en todos los campos
- Manejo de errores centralizado y en español
- Arquitectura limpia con separación de responsabilidades
- Interfaz de usuario profesional y fácil de usar
- POO avanzada con interfaces e inyección de dependencias

---

**Desarrollado por:** Jerónimo Gutiérrez Arias  
**Clan:** Van Rossum  
**Correo:** jeronimogutierrezarias@outlook.com  
**Documento:** 1021805938