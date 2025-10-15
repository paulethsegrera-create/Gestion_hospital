using System;
using Gestion_Hospital.Models;
using Gestion_Hospital.Services;
using Gestion_Hospital.Utils;

namespace Gestion_Hospital.Controllers
{
    // Controller responsible for handling all operations related to patients
    public class PatientController
    {
    // Business service that manages patient logic
        private readonly PatientService _service;

    // Constructor that injects the patient service
        public PatientController(PatientService service)
        {
            _service = service;
        }

    // ======================================
    // METHOD: Register a new patient
    // ======================================
        public void Create()
        {
            Console.WriteLine("=== Registrar Paciente ===");
            try
            {
                // Requests patient data using the input helper with integrated validations
                var name = InputHelper.PromptName("Nombre: ");
                var doc = InputHelper.PromptDocument("Documento: ");
                var age = InputHelper.PromptAge("Edad: ");
                var (phoneNormalized, truncatedPhone) = InputHelper.PromptPhone("Teléfono: ");
                var emailNormalized = InputHelper.PromptEmail("Correo: ");

                // -------------------------
                // Strict validations
                // -------------------------
                if (!Validator.IsValidName(name))
                {
                    Console.WriteLine("❌ Nombre inválido. El registro fue cancelado.");
                    return;
                }
                if (!Validator.IsValidDocument(doc))
                {
                    Console.WriteLine("❌ Documento inválido. El registro fue cancelado.");
                    return;
                }
                if (!Validator.IsValidAge(age))
                {
                    Console.WriteLine("❌ Edad inválida. El registro fue cancelado.");
                    return;
                }
                if (!Validator.IsValidPhone(phoneNormalized))
                {
                    Console.WriteLine("❌ Teléfono inválido. Debe contener entre 7 y 10 dígitos. El registro fue cancelado.");
                    return;
                }
                if (!Validator.IsValidEmail(emailNormalized))
                {
                    Console.WriteLine("❌ Correo inválido (formato). El registro fue cancelado.");
                    return;
                }

                // Warning message if the phone number was automatically truncated
                if (truncatedPhone)
                    Console.WriteLine("⚠️ El teléfono ingresado era más largo de 10 dígitos y fue truncado a los 10 dígitos finales.");

                // Creates the patient object with validated data
                var patient = new Patient
                {
                    Name = name.Trim(),
                    Document = doc.Trim(),
                    Age = age,
                    PhoneNumber = phoneNormalized,
                    Email = emailNormalized
                };

                // Calls the service to register the new patient
                _service.RegisterPatient(patient);
                Console.WriteLine("✅ Paciente registrado correctamente.");
            }
            catch (Exception ex)
            {
                // Generic error handling
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

    // ======================================
    // METHOD: Edit patient data
    // ======================================
        public void Edit()
        {
            Console.WriteLine("=== Editar Paciente ===");
            try
            {
                // Requests the document of the patient to edit
                var doc = InputHelper.PromptString("Documento del paciente a editar: ", false).Trim();
                var existing = _service.GetByDocument(doc);

                // If the patient is not found, editing is interrupted
                if (existing == null) { Console.WriteLine("Paciente no encontrado."); return; }

                // Shows current values and allows entering new optional values
                Console.Write($"Nombre ({existing.Name}): "); var name = InputHelper.PromptString("", true);
                Console.Write($"Edad ({existing.Age}): "); var ageStr = InputHelper.PromptString("", true);
                Console.Write($"Teléfono ({existing.PhoneNumber}): "); var phoneInput = InputHelper.PromptString("", true);
                Console.Write($"Correo ({existing.Email}): "); var emailInput = InputHelper.PromptString("", true);

                // If the user entered a new name, it is validated and replaced
                if (!string.IsNullOrWhiteSpace(name))
                {
                    // Uses the PromptName method to ensure retries until a valid name is entered
                    var validName = InputHelper.PromptName("Nombre: ");
                    existing.Name = validName;
                }

                // If a new age is entered, it is converted and validated
                if (!string.IsNullOrWhiteSpace(ageStr) && int.TryParse(ageStr, out int newAge))
                {
                    if (!Validator.IsValidAge(newAge))
                    {
                        Console.WriteLine("❌ Edad inválida. La actualización fue cancelada.");
                        return;
                    }
                    existing.Age = newAge;
                }

                // If a new phone is entered, it is validated and normalized
                if (!string.IsNullOrWhiteSpace(phoneInput))
                {
                    // PromptPhone forces retries until the phone is valid
                    var (normalized, _) = InputHelper.PromptPhone("Teléfono: ");
                    existing.PhoneNumber = normalized;
                }

                // If a new email is entered, it is also validated using InputHelper
                if (!string.IsNullOrWhiteSpace(emailInput))
                {
                    var normalized = InputHelper.PromptEmail("Correo: ");
                    existing.Email = normalized;
                }

                // Saves the changes in the service
                _service.UpdatePatient(existing);
                Console.WriteLine("✅ Paciente actualizado.");
            }
            catch (Exception ex)
            {
                // Captures any error in the process
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

    // ======================================
    // METHOD: List all patients
    // ======================================
        public void List()
        {
            Console.WriteLine("=== Lista de Pacientes ===");

            // Iterates all patients and displays their information in masked form
            foreach (var p in _service.GetAll())
            {
                // Display masked sensitive data
                Console.WriteLine($"Id:{p.Id} | {p.Name} | Doc:{p.MaskDocument()} | Edad:{p.Age} | Tel:{p.MaskPhone()} | Email:{p.Email}");
            }
        }

    // ======================================
    // METHOD: Delete patient by document
    // ======================================
        public void Delete()
        {
            Console.WriteLine("=== Eliminar Paciente ===");

            // Requests the document of the patient to delete
            var doc = InputHelper.PromptDocument("Documento del paciente a eliminar: ");
            var existing = _service.GetByDocument(doc);

            // Checks if the patient exists before deleting
            if (existing == null)
            {
                Console.WriteLine("Paciente no encontrado.");
                return;
            }

            // Deletes the patient through the service using their ID
            _service.DeletePatient(existing.Id);
            Console.WriteLine($"Paciente eliminado: {existing.Name} ({existing.MaskDocument()})");
        }
    }
}
