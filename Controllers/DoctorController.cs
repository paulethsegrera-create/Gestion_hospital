using System;
using Gestion_Hospital.Models;
using Gestion_Hospital.Services;
using Gestion_Hospital.Utils;

namespace Gestion_Hospital.Controllers
{
    // Controller responsible for managing all operations related to doctors.
    public class DoctorController
    {
    // Business service that handles doctor logic.
        private readonly DoctorService _service;

    // Constructor that receives the service via dependency injection.
        public DoctorController(DoctorService service)
        {
            _service = service;
        }

    // ===============================
    // METHOD: Create new doctor
    // ===============================
        public void Create()
        {
            Console.WriteLine("=== Registrar Médico ===");
            try
            {
                // Doctor data is requested using the input helper (InputHelper)
                var name = InputHelper.PromptName("Nombre: ");
                var doc = InputHelper.PromptDocument("Documento: ");
                var specialty = InputHelper.PromptString("Especialidad: ", false).Trim();
                var (phoneNormalized, truncatedPhone) = InputHelper.PromptPhone("Teléfono: ");
                var emailNormalized = InputHelper.PromptEmail("Correo: ");

                // Strict validations for each field before registering the doctor
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

                // Warning if the phone number was automatically truncated
                if (truncatedPhone)
                    Console.WriteLine("⚠️ El teléfono ingresado era más largo de 10 dígitos y fue truncado a los 10 dígitos finales.");

                // Doctor object is created with validated data
                var doctor = new Doctor
                {
                    Name = name.Trim(),
                    Document = doc.Trim(),
                    Specialty = specialty.Trim(),
                    PhoneNumber = phoneNormalized,
                    Email = emailNormalized
                };

                // Doctor is registered using the service
                _service.RegisterDoctor(doctor);
                Console.WriteLine("✅ Médico registrado correctamente.");
            }
            catch (Exception ex)
            {
                // Captures and displays errors occurred during registration
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

    // ===============================
    // METHOD: Edit existing doctor
    // ===============================
        public void Edit()
        {
            Console.WriteLine("=== Editar Médico ===");
            try
            {
                // Requests the document to locate the existing doctor
                var doc = InputHelper.PromptString("Documento del médico a editar: ", false).Trim();
                var existing = _service.GetAll().Find(d => d.Document == doc);
                if (existing == null) { Console.WriteLine("Médico no encontrado."); return; }

                // Shows current values and allows entering new ones (optionally)
                Console.Write($"Nombre ({existing.Name}): "); var name = InputHelper.PromptString("", true);
                Console.Write($"Especialidad ({existing.Specialty}): "); var specialty = InputHelper.PromptString("", true);
                Console.Write($"Teléfono ({existing.PhoneNumber}): "); var phoneInput = InputHelper.PromptString("", true);
                Console.Write($"Correo ({existing.Email}): "); var emailInput = InputHelper.PromptString("", true);

                // Name validation and update
                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (!Validator.IsValidName(name))
                    {
                        Console.WriteLine("❌ Nombre inválido. La actualización fue cancelada.");
                        return;
                    }
                    existing.Name = name.Trim();
                }

                // Direct update of specialty (no extra validation)
                if (!string.IsNullOrWhiteSpace(specialty)) existing.Specialty = specialty.Trim();

                // Phone number validation and normalization
                if (!string.IsNullOrWhiteSpace(phoneInput))
                {
                    var normalized = Validator.NormalizePhone(phoneInput, out bool truncated);
                    if (!Validator.IsValidPhone(normalized))
                    {
                        Console.WriteLine("❌ Teléfono inválido. Debe contener entre 7 y 10 dígitos. La actualización fue cancelada.");
                        return;
                    }
                    if (truncated)
                        Console.WriteLine("⚠️ El teléfono ingresado era más largo de 10 dígitos y fue truncado a los 10 dígitos finales.");

                    existing.PhoneNumber = normalized;
                }

                // Email validation and normalization
                if (!string.IsNullOrWhiteSpace(emailInput))
                {
                    var emailNormalized = Validator.NormalizeEmail(emailInput);
                    if (!Validator.IsValidEmail(emailNormalized))
                    {
                        Console.WriteLine("❌ Correo inválido (formato). La actualización fue cancelada.");
                        return;
                    }
                    existing.Email = emailNormalized;
                }

                // The update is saved in the service
                _service.UpdateDoctor(existing);
                Console.WriteLine("✅ Médico actualizado.");
            }
            catch (Exception ex)
            {
                // Captures general errors
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

    // ===============================
    // METHOD: List all doctors
    // ===============================
        public void List()
        {
            Console.WriteLine("=== Lista de Médicos ===");

            // Iterates all doctors and displays their basic information
            foreach (var d in _service.GetAll())
            {
                    Console.WriteLine($"Id:{d.Id} | {d.Name} | Esp:{d.Specialty} | Doc:{d.MaskDocument()} | Tel:{d.MaskPhone()} | Email:{d.Email}");
            }
        }

    // ===============================
    // METHOD: List doctors by specialty
    // ===============================
        public void ListBySpecialty()
        {
            Console.Write("Ingrese especialidad para filtrar: ");
            var spec = Console.ReadLine() ?? "";

            // Gets doctors matching the given specialty
            var list = _service.GetBySpecialty(spec);

            Console.WriteLine($"=== Médicos - filtro: {spec} ===");
            foreach (var d in list)
            {
                // Displays data with masked document
                Console.WriteLine($"Id:{d.Id} | {d.Name} | Esp:{d.Specialty} | Doc:{d.MaskDocument()}");
            }
        }

    // ===============================
    // METHOD: Delete a doctor
    // ===============================
        public void Delete()
        {
            Console.WriteLine("=== Eliminar Médico ===");

            // Requests document of the doctor to delete
            var doc = InputHelper.PromptDocument("Documento del médico a eliminar: ");
            var existing = _service.GetAll().Find(d => d.Document == doc);

            if (existing == null)
            {
                Console.WriteLine("Médico no encontrado.");
                return;
            }
                    Console.Write($"Teléfono ({existing.MaskPhone()}): "); var phoneInput = InputHelper.PromptString("", true);
            // Deletes the doctor using their Id
            _service.DeleteDoctor(existing.Id);
            Console.WriteLine($"Médico eliminado: {existing.Name} ({existing.MaskDocument()})");
        }
    }
}
