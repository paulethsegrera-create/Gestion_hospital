using System;
using Gestion_Hospital.Models;
using Gestion_Hospital.Services;
using Gestion_Hospital.Utils;

namespace Gestion_Hospital.Controllers
{
    // Controlador encargado de gestionar todas las operaciones relacionadas con los médicos.
    public class DoctorController
    {
        // Servicio de negocio que maneja la lógica de los doctores.
        private readonly DoctorService _service;

        // Constructor que recibe el servicio por inyección de dependencias.
        public DoctorController(DoctorService service)
        {
            _service = service;
        }

        // ===============================
        // MÉTODO: Crear nuevo médico
        // ===============================
        public void Create()
        {
            Console.WriteLine("=== Registrar Médico ===");
            try
            {
                // Se solicitan los datos del médico utilizando el helper de entrada (InputHelper)
                var name = InputHelper.PromptName("Nombre: ");
                var doc = InputHelper.PromptDocument("Documento: ");
                var specialty = InputHelper.PromptString("Especialidad: ", false).Trim();
                var (phoneNormalized, truncatedPhone) = InputHelper.PromptPhone("Teléfono: ");
                var emailNormalized = InputHelper.PromptEmail("Correo: ");

                // Validaciones estrictas de cada campo antes de registrar el médico
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

                // Aviso si el número telefónico fue truncado automáticamente
                if (truncatedPhone)
                    Console.WriteLine("⚠️ El teléfono ingresado era más largo de 10 dígitos y fue truncado a los 10 dígitos finales.");

                // Se crea el objeto Doctor con los datos validados
                var doctor = new Doctor
                {
                    Name = name.Trim(),
                    Document = doc.Trim(),
                    Specialty = specialty.Trim(),
                    PhoneNumber = phoneNormalized,
                    Email = emailNormalized
                };

                // Se registra el médico usando el servicio
                _service.RegisterDoctor(doctor);
                Console.WriteLine("✅ Médico registrado correctamente.");
            }
            catch (Exception ex)
            {
                // Captura y muestra errores ocurridos durante el registro
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        // ===============================
        // MÉTODO: Editar médico existente
        // ===============================
        public void Edit()
        {
            Console.WriteLine("=== Editar Médico ===");
            try
            {
                // Solicita el documento para localizar al médico existente
                var doc = InputHelper.PromptString("Documento del médico a editar: ", false).Trim();
                var existing = _service.GetAll().Find(d => d.Document == doc);
                if (existing == null) { Console.WriteLine("Médico no encontrado."); return; }

                // Muestra los valores actuales y permite ingresar nuevos (opcionalmente)
                Console.Write($"Nombre ({existing.Name}): "); var name = InputHelper.PromptString("", true);
                Console.Write($"Especialidad ({existing.Specialty}): "); var specialty = InputHelper.PromptString("", true);
                Console.Write($"Teléfono ({existing.PhoneNumber}): "); var phoneInput = InputHelper.PromptString("", true);
                Console.Write($"Correo ({existing.Email}): "); var emailInput = InputHelper.PromptString("", true);

                // Validación y actualización del nombre
                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (!Validator.IsValidName(name))
                    {
                        Console.WriteLine("❌ Nombre inválido. La actualización fue cancelada.");
                        return;
                    }
                    existing.Name = name.Trim();
                }

                // Actualización directa de la especialidad (sin validación extra)
                if (!string.IsNullOrWhiteSpace(specialty)) existing.Specialty = specialty.Trim();

                // Validación y normalización del número de teléfono
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

                // Validación y normalización del correo electrónico
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

                // Se guarda la actualización en el servicio
                _service.UpdateDoctor(existing);
                Console.WriteLine("✅ Médico actualizado.");
            }
            catch (Exception ex)
            {
                // Captura errores generales
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        // ===============================
        // MÉTODO: Listar todos los médicos
        // ===============================
        public void List()
        {
            Console.WriteLine("=== Lista de Médicos ===");

            // Recorre todos los médicos y muestra su información básica
            foreach (var d in _service.GetAll())
            {
                    Console.WriteLine($"Id:{d.Id} | {d.Name} | Esp:{d.Specialty} | Doc:{d.MaskDocument()} | Tel:{d.MaskPhone()} | Email:{d.Email}");
            }
        }

        // ===============================
        // MÉTODO: Listar médicos por especialidad
        // ===============================
        public void ListBySpecialty()
        {
            Console.Write("Ingrese especialidad para filtrar: ");
            var spec = Console.ReadLine() ?? "";

            // Obtiene los médicos que coincidan con la especialidad dada
            var list = _service.GetBySpecialty(spec);

            Console.WriteLine($"=== Médicos - filtro: {spec} ===");
            foreach (var d in list)
            {
                // Muestra datos con el documento enmascarado
                Console.WriteLine($"Id:{d.Id} | {d.Name} | Esp:{d.Specialty} | Doc:{d.MaskDocument()}");
            }
        }

        // ===============================
        // MÉTODO: Eliminar un médico
        // ===============================
        public void Delete()
        {
            Console.WriteLine("=== Eliminar Médico ===");

            // Solicita documento del médico a eliminar
            var doc = InputHelper.PromptDocument("Documento del médico a eliminar: ");
            var existing = _service.GetAll().Find(d => d.Document == doc);

            if (existing == null)
            {
                Console.WriteLine("Médico no encontrado.");
                return;
            }
                    Console.Write($"Teléfono ({existing.MaskPhone()}): "); var phoneInput = InputHelper.PromptString("", true);
            // Elimina el médico usando su Id
            _service.DeleteDoctor(existing.Id);
            Console.WriteLine($"Médico eliminado: {existing.Name} ({existing.MaskDocument()})");
        }
    }
}
