using System;
using Gestion_Hospital.Controllers;
using Gestion_Hospital.Repositories;
using Gestion_Hospital.Services;
using Gestion_Hospital.Interfaces;
using Microsoft.Extensions.Logging;

class Program
{
    static void Main()
    {
    // --- Logger ---
    using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

    // --- Dependencias simuladas ---
    IPatientRepository patientRepo = new PatientRepository();
    IDoctorRepository doctorRepo = new DoctorRepository();
    IAppointmentRepository appointmentRepo = new AppointmentRepository();
    IEmailService emailService = new EmailService(loggerFactory.CreateLogger<Gestion_Hospital.Services.EmailService>());

    // --- Servicios ---
    var appointmentService = new AppointmentService(appointmentRepo, patientRepo, doctorRepo, emailService, loggerFactory.CreateLogger<Gestion_Hospital.Services.AppointmentService>());
    var patientService = new PatientService(patientRepo, appointmentService, loggerFactory.CreateLogger<Gestion_Hospital.Services.PatientService>());
    var doctorService = new DoctorService(doctorRepo, appointmentService, loggerFactory.CreateLogger<Gestion_Hospital.Services.DoctorService>());

    // --- Controladores ---
    var patientController = new PatientController(patientService);
    var doctorController = new DoctorController(doctorService);
    var appointmentController = new AppointmentController(appointmentService, patientService, doctorService);

        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║  SISTEMA DE GESTIÓN HOSPITALARIA  ║");
            Console.WriteLine("╠════════════════════════════════════╣");
            Console.WriteLine("║ 1. Gestión de Pacientes            ║");
            Console.WriteLine("║ 2. Gestión de Médicos              ║");
            Console.WriteLine("║ 3. Gestión de Citas Médicas        ║");
            Console.WriteLine("║ 4. Historial de Correos            ║");
            Console.WriteLine("║ 0. Salir                           ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("Seleccione una opción: ");

            var opt = Console.ReadLine();

            switch (opt)
            {
                case "1":
                    MenuPacientes(patientController);
                    break;
                case "2":
                    MenuMedicos(doctorController);
                    break;
                case "3":
                    MenuCitas(appointmentController);
                    break;
                case "4":
                    MostrarHistorial(emailService);
                    break;
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("⚠️  Opción inválida. Presione Enter para continuar...");
                    Console.ReadLine();
                    break;
            }
        }

        Console.WriteLine("👋 Saliendo del sistema...");
    }

    // --- SUBMENÚ DE PACIENTES ---
    static void MenuPacientes(PatientController controller)
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("\n=== GESTIÓN DE PACIENTES ===");
            Console.WriteLine("1. Registrar nuevo paciente");
            Console.WriteLine("2. Editar paciente");
            Console.WriteLine("3. Listar pacientes");
            Console.WriteLine("4. Eliminar paciente");
            Console.WriteLine("0. Volver al menú principal");
            Console.Write("Opción: ");
            var opt = Console.ReadLine();

            try
            {
                switch (opt)
                {
                    case "1": controller.Create(); break;
                    case "2": controller.Edit(); break;
                    case "3": controller.List(); break;
                    case "4": controller.Delete(); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("Opción inválida."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }

            if (!back)
            {
                Console.WriteLine("\nPresione Enter para continuar...");
                Console.ReadLine();
            }
        }
    }

    // --- SUBMENÚ DE MÉDICOS ---
    static void MenuMedicos(DoctorController controller)
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("\n=== GESTIÓN DE MÉDICOS ===");
            Console.WriteLine("1. Registrar nuevo médico");
            Console.WriteLine("2. Editar médico");
            Console.WriteLine("3. Listar todos");
            Console.WriteLine("4. Filtrar por especialidad");
            Console.WriteLine("5. Eliminar médico");
            Console.WriteLine("0. Volver al menú principal");
            Console.Write("Opción: ");
            var opt = Console.ReadLine();

            try
            {
                switch (opt)
                {
                    case "1": controller.Create(); break;
                    case "2": controller.Edit(); break;
                    case "3": controller.List(); break;
                    case "4": controller.ListBySpecialty(); break;
                    case "5": controller.Delete(); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("Opción inválida."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }

            if (!back)
            {
                Console.WriteLine("\nPresione Enter para continuar...");
                Console.ReadLine();
            }
        }
    }

    // --- SUBMENÚ DE CITAS ---
    static void MenuCitas(AppointmentController controller)
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("\n=== GESTIÓN DE CITAS MÉDICAS ===");
            Console.WriteLine("1. Agendar cita");
            Console.WriteLine("2. Cancelar cita");
            Console.WriteLine("3. Marcar cita como atendida");
            Console.WriteLine("4. Listar por paciente");
            Console.WriteLine("5. Listar por médico");
            Console.WriteLine("6. Eliminar cita");
            Console.WriteLine("0. Volver al menú principal");
            Console.Write("Opción: ");
            var opt = Console.ReadLine();

            try
            {
                switch (opt)
                {
                    case "1": controller.Create(); break;
                    case "2": controller.Cancel(); break;
                    case "3": controller.MarkAttended(); break;
                    case "4": controller.ListByPatient(); break;
                    case "5": controller.ListByDoctor(); break;
                    case "6": controller.Delete(); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("Opción inválida."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }

            if (!back)
            {
                Console.WriteLine("\nPresione Enter para continuar...");
                Console.ReadLine();
            }
        }
    }

    // --- HISTORIAL DE CORREOS ---
    static void MostrarHistorial(IEmailService emailService)
    {
        Console.Clear();
        var history = emailService.GetHistory();
        Console.WriteLine("=== HISTORIAL DE CORREOS ===");
        foreach (var h in history)
        {
            Console.WriteLine($"Id: {h.Id} | To: {h.To} | Enviado: {h.Sent} | Fecha: {h.Timestamp} | Error: {h.ErrorMessage}");
        }
        Console.WriteLine("\nPresione Enter para volver al menú principal...");
        Console.ReadLine();
    }
}
