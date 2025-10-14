using System;
using Gestion_Hospital.Models;
using Gestion_Hospital.Services;
using Gestion_Hospital.Utils;

namespace Gestion_Hospital.Controllers
{
    public class DoctorController
    {
        private readonly DoctorService _service;

        public DoctorController(DoctorService service)
        {
            _service = service;
        }

        public void Create()
        {
            Console.WriteLine("=== Registrar Médico ===");
            try
            {
                var name = InputHelper.PromptName("Nombre: ");
                var doc = InputHelper.PromptDocument("Documento: ");
                var specialty = InputHelper.PromptString("Especialidad: ", false).Trim();
                var (phoneNormalized, truncatedPhone) = InputHelper.PromptPhone("Teléfono: ");
                var emailNormalized = InputHelper.PromptEmail("Correo: ");

                // Validaciones estrictas: si alguna falla, no crear el registro
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

                if (truncatedPhone)
                    Console.WriteLine("⚠️ El teléfono ingresado era más largo de 10 dígitos y fue truncado a los 10 dígitos finales.");

                var doctor = new Doctor { Name = name.Trim(), Document = doc.Trim(), Specialty = specialty.Trim(), PhoneNumber = phoneNormalized, Email = emailNormalized };
                _service.RegisterDoctor(doctor);
                Console.WriteLine("✅ Médico registrado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        public void Edit()
        {
            Console.WriteLine("=== Editar Médico ===");
            try
            {
                var doc = InputHelper.PromptString("Documento del médico a editar: ", false).Trim();
                var existing = _service.GetAll().Find(d => d.Document == doc);
                if (existing == null) { Console.WriteLine("Médico no encontrado."); return; }
                Console.Write($"Nombre ({existing.Name}): "); var name = InputHelper.PromptString("", true);
                Console.Write($"Especialidad ({existing.Specialty}): "); var specialty = InputHelper.PromptString("", true);
                Console.Write($"Teléfono ({existing.PhoneNumber}): "); var phoneInput = InputHelper.PromptString("", true);
                Console.Write($"Correo ({existing.Email}): "); var emailInput = InputHelper.PromptString("", true);

                // Si el usuario ingresa valores nuevos, validarlos estrictamente.
                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (!Validator.IsValidName(name))
                    {
                        Console.WriteLine("❌ Nombre inválido. La actualización fue cancelada.");
                        return;
                    }
                    existing.Name = name.Trim();
                }

                if (!string.IsNullOrWhiteSpace(specialty)) existing.Specialty = specialty.Trim();

                if (!string.IsNullOrWhiteSpace(phoneInput))
                {
                    var normalized = Validator.NormalizePhone(phoneInput, out bool truncated);
                    if (!Validator.IsValidPhone(normalized))
                    {
                        Console.WriteLine("❌ Teléfono inválido. Debe contener entre 7 y 10 dígitos. La actualización fue cancelada.");
                        return;
                    }
                    if (truncated) Console.WriteLine("⚠️ El teléfono ingresado era más largo de 10 dígitos y fue truncado a los 10 dígitos finales.");
                    existing.PhoneNumber = normalized;
                }

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

                _service.UpdateDoctor(existing);
                Console.WriteLine("✅ Médico actualizado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        public void List()
        {
            Console.WriteLine("=== Lista de Médicos ===");
            foreach (var d in _service.GetAll())
            {
                Console.WriteLine($"Id:{d.Id} | {d.Name} | Esp:{d.Specialty} | Doc:{d.Document} | Tel:{d.PhoneNumber} | Email:{d.Email}");
            }
        }

        public void ListBySpecialty()
        {
            Console.Write("Ingrese especialidad para filtrar: ");
            var spec = Console.ReadLine() ?? "";
            var list = _service.GetBySpecialty(spec);
            Console.WriteLine($"=== Médicos - filtro: {spec} ===");
            foreach (var d in list)
            {
                Console.WriteLine($"Id:{d.Id} | {d.Name} | Esp:{d.Specialty} | Doc:{d.MaskDocument()}");
            }
        }

        public void Delete()
        {
            Console.WriteLine("=== Eliminar Médico ===");
            var doc = InputHelper.PromptDocument("Documento del médico a eliminar: ");
            var existing = _service.GetAll().Find(d => d.Document == doc);
            if (existing == null) { Console.WriteLine("Médico no encontrado."); return; }
            _service.DeleteDoctor(existing.Id);
            Console.WriteLine($"Médico eliminado: {existing.Name} ({existing.MaskDocument()})");
        }
    }
}
