using System;
using Gestion_Hospital.Models;
using Gestion_Hospital.Services;
using Gestion_Hospital.Utils;

namespace Gestion_Hospital.Controllers
{
    public class PatientController
    {
        private readonly PatientService _service;

        public PatientController(PatientService service)
        {
            _service = service;
        }

        public void Create()
        {
            Console.WriteLine("=== Registrar Paciente ===");
            try
            {
                var name = InputHelper.PromptName("Nombre: ");
                var doc = InputHelper.PromptDocument("Documento: ");
                var age = InputHelper.PromptAge("Edad: ");
                var (phoneNormalized, truncatedPhone) = InputHelper.PromptPhone("Teléfono: ");
                var emailNormalized = InputHelper.PromptEmail("Correo: ");

                // Validaciones estrictas
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

                if (truncatedPhone)
                    Console.WriteLine("⚠️ El teléfono ingresado era más largo de 10 dígitos y fue truncado a los 10 dígitos finales.");

                var patient = new Patient { Name = name.Trim(), Document = doc.Trim(), Age = age, PhoneNumber = phoneNormalized, Email = emailNormalized };
                _service.RegisterPatient(patient);
                Console.WriteLine("✅ Paciente registrado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        public void Edit()
        {
            Console.WriteLine("=== Editar Paciente ===");
            try
            {
                var doc = InputHelper.PromptString("Documento del paciente a editar: ", false).Trim();
                var existing = _service.GetByDocument(doc);
                if (existing == null) { Console.WriteLine("Paciente no encontrado."); return; }
                Console.Write($"Nombre ({existing.Name}): "); var name = InputHelper.PromptString("", true);
                Console.Write($"Edad ({existing.Age}): "); var ageStr = InputHelper.PromptString("", true);
                Console.Write($"Teléfono ({existing.PhoneNumber}): "); var phoneInput = InputHelper.PromptString("", true);
                Console.Write($"Correo ({existing.Email}): "); var emailInput = InputHelper.PromptString("", true);

                if (!string.IsNullOrWhiteSpace(name))
                {
                    // Si el usuario ingresó nombre en el prompt de edición, validarlo (reintentos)
                    var validName = InputHelper.PromptName("Nombre: ");
                    existing.Name = validName;
                }
                if (!string.IsNullOrWhiteSpace(ageStr) && int.TryParse(ageStr, out int newAge))
                {
                    if (!Validator.IsValidAge(newAge))
                    {
                        Console.WriteLine("❌ Edad inválida. La actualización fue cancelada.");
                        return;
                    }
                    existing.Age = newAge;
                }
                if (!string.IsNullOrWhiteSpace(phoneInput))
                {
                    // Si el usuario ingresó algo, usar PromptPhone para forzar reintentos hasta que sea válido
                    var (normalized, _) = InputHelper.PromptPhone("Teléfono: ");
                    existing.PhoneNumber = normalized;
                }

                if (!string.IsNullOrWhiteSpace(emailInput))
                {
                    var normalized = InputHelper.PromptEmail("Correo: ");
                    existing.Email = normalized;
                }

                _service.UpdatePatient(existing);
                Console.WriteLine("✅ Paciente actualizado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        public void List()
        {
            Console.WriteLine("=== Lista de Pacientes ===");
            foreach (var p in _service.GetAll())
            {
                Console.WriteLine($"Id:{p.Id} | {p.Name} | Doc:{p.MaskDocument()} | Edad:{p.Age} | Tel:{p.MaskPhone()} | Email:{p.Email}");
            }
        }

        public void Delete()
        {
            Console.WriteLine("=== Eliminar Paciente ===");
            var doc = InputHelper.PromptDocument("Documento del paciente a eliminar: ");
            var existing = _service.GetByDocument(doc);
            if (existing == null) { Console.WriteLine("Paciente no encontrado."); return; }
            _service.DeletePatient(existing.Id);
            Console.WriteLine($"Paciente eliminado: {existing.Name} ({existing.MaskDocument()})");
        }
    }
}
