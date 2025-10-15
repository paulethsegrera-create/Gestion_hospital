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

    // --- DataContext ---
    var dataContext = new Gestion_Hospital.DataBase.DataContext();

    // --- Services ---
    IEmailService emailService = new EmailService(loggerFactory.CreateLogger<Gestion_Hospital.Services.EmailService>());
    var appointmentService = new AppointmentService(
        dataContext.AppointmentRepository,
        dataContext.PatientRepository,
        dataContext.DoctorRepository,
        emailService,
        loggerFactory.CreateLogger<Gestion_Hospital.Services.AppointmentService>());
    var patientService = new PatientService(dataContext.PatientRepository, appointmentService, loggerFactory.CreateLogger<Gestion_Hospital.Services.PatientService>());
    var doctorService = new DoctorService(dataContext.DoctorRepository, appointmentService, loggerFactory.CreateLogger<Gestion_Hospital.Services.DoctorService>());

    // --- Controllers ---
    var patientController = new PatientController(patientService);
    var doctorController = new DoctorController(doctorService);
    var appointmentController = new AppointmentController(appointmentService, patientService, doctorService);

        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║  HOSPITAL MANAGEMENT SYSTEM        ║");
            Console.WriteLine("╠════════════════════════════════════╣");
            Console.WriteLine("║ 1. Patient Management              ║");
            Console.WriteLine("║ 2. Doctor Management               ║");
            Console.WriteLine("║ 3. Medical Appointment Management  ║");
            Console.WriteLine("║ 4. Email History                   ║");
            Console.WriteLine("║ 0. Exit                            ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("Select an option: ");

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
                    Console.WriteLine("⚠️  Invalid option. Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }

    Console.WriteLine("👋 Exiting the system...");
    }

    // --- PATIENT SUBMENU ---
    static void MenuPacientes(PatientController controller)
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("\n=== PATIENT MANAGEMENT ===");
            Console.WriteLine("1. Register new patient");
            Console.WriteLine("2. Edit patient");
            Console.WriteLine("3. List patients");
            Console.WriteLine("4. Delete patient");
            Console.WriteLine("0. Return to main menu");
            Console.Write("Option: ");
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
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }

            if (!back)
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }

    // --- DOCTOR SUBMENU ---
    static void MenuMedicos(DoctorController controller)
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("\n=== DOCTOR MANAGEMENT ===");
            Console.WriteLine("1. Register new doctor");
            Console.WriteLine("2. Edit doctor");
            Console.WriteLine("3. List all");
            Console.WriteLine("4. Filter by specialty");
            Console.WriteLine("5. Delete doctor");
            Console.WriteLine("0. Return to main menu");
            Console.Write("Option: ");
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
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }

            if (!back)
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }

    // --- APPOINTMENT SUBMENU ---
    static void MenuCitas(AppointmentController controller)
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("\n=== MEDICAL APPOINTMENT MANAGEMENT ===");
            Console.WriteLine("1. Schedule appointment");
            Console.WriteLine("2. Cancel appointment");
            Console.WriteLine("3. Mark appointment as attended");
            Console.WriteLine("4. List by patient");
            Console.WriteLine("5. List by doctor");
            Console.WriteLine("6. Delete appointment");
            Console.WriteLine("0. Return to main menu");
            Console.Write("Option: ");
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
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }

            if (!back)
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }

    // --- EMAIL HISTORY ---
    static void MostrarHistorial(IEmailService emailService)
    {
        Console.Clear();
        var history = emailService.GetHistory();
        Console.WriteLine("=== EMAIL HISTORY ===");
        foreach (var h in history)
        {
            Console.WriteLine($"Id: {h.Id} | To: {h.To} | Sent: {h.Sent} | Date: {h.Timestamp} | Error: {h.ErrorMessage}");
        }
        Console.WriteLine("\nPress Enter to return to the main menu...");
        Console.ReadLine();
    }
}
