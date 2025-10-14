using System;
using Gestion_Hospital.Models;
using Gestion_Hospital.Services;
using Gestion_Hospital.Utils;

namespace Gestion_Hospital.Controllers
{
    // Controlador encargado de manejar todas las operaciones relacionadas con los pacientes
    public class PatientController
    {
        // Servicio de negocio que gestiona la lógica de los pacientes
        private readonly PatientService _service;

        // Constructor que inyecta el servicio de pacientes
        public PatientController(PatientService service)
        {
            _service = service;
        }

        // ======================================
        // MÉTODO: Registrar un nuevo paciente
        // ======================================
        public void Create()
        {
            Console.WriteLine("=== Registrar Paciente ===");
            try
            {
                // Solicita los datos del paciente utilizando el helper de entrada con validaciones integradas
                var name = InputHelper.PromptName("Nombre: ");
                var doc = InputHelper.PromptDocument("Documento: ");
                var age = InputHelper.PromptAge("Edad: ");
                var (phoneNormalized, truncatedPhone) = InputHelper.PromptPhone("Teléfono: ");
                var emailNormalized = InputHelper.PromptEmail("Correo: ");

                // -------------------------
                // Validaciones estrictas
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

                // Mensaje de advertencia si el número de teléfono fue truncado automáticamente
                if (truncatedPhone)
                    Console.WriteLine("⚠️ El teléfono ingresado era más largo de 10 dígitos y fue truncado a los 10 dígitos finales.");

                // Crea el objeto paciente con los datos validados
                var patient = new Patient
                {
                    Name = name.Trim(),
                    Document = doc.Trim(),
                    Age = age,
                    PhoneNumber = phoneNormalized,
                    Email = emailNormalized
                };

                // Llama al servicio para registrar el nuevo paciente
                _service.RegisterPatient(patient);
                Console.WriteLine("✅ Paciente registrado correctamente.");
            }
            catch (Exception ex)
            {
                // Manejo de errores genérico
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        // ======================================
        // MÉTODO: Editar datos de un paciente
        // ======================================
        public void Edit()
        {
            Console.WriteLine("=== Editar Paciente ===");
            try
            {
                // Solicita el documento del paciente a editar
                var doc = InputHelper.PromptString("Documento del paciente a editar: ", false).Trim();
                var existing = _service.GetByDocument(doc);

                // Si no se encuentra el paciente, se interrumpe la edición
                if (existing == null) { Console.WriteLine("Paciente no encontrado."); return; }

                // Muestra los valores actuales y permite ingresar nuevos valores opcionales
                Console.Write($"Nombre ({existing.Name}): "); var name = InputHelper.PromptString("", true);
                Console.Write($"Edad ({existing.Age}): "); var ageStr = InputHelper.PromptString("", true);
                Console.Write($"Teléfono ({existing.PhoneNumber}): "); var phoneInput = InputHelper.PromptString("", true);
                Console.Write($"Correo ({existing.Email}): "); var emailInput = InputHelper.PromptString("", true);

                // Si el usuario ingresó un nuevo nombre, se valida y reemplaza
                if (!string.IsNullOrWhiteSpace(name))
                {
                    // Usa el método PromptName para asegurar reintentos hasta un nombre válido
                    var validName = InputHelper.PromptName("Nombre: ");
                    existing.Name = validName;
                }

                // Si se ingresó una nueva edad, se convierte y valida
                if (!string.IsNullOrWhiteSpace(ageStr) && int.TryParse(ageStr, out int newAge))
                {
                    if (!Validator.IsValidAge(newAge))
                    {
                        Console.WriteLine("❌ Edad inválida. La actualización fue cancelada.");
                        return;
                    }
                    existing.Age = newAge;
                }

                // Si se ingresó un nuevo teléfono, se valida y normaliza
                if (!string.IsNullOrWhiteSpace(phoneInput))
                {
                    // PromptPhone fuerza reintentos hasta que el teléfono sea válido
                    var (normalized, _) = InputHelper.PromptPhone("Teléfono: ");
                    existing.PhoneNumber = normalized;
                }

                // Si se ingresó un nuevo correo, se valida también mediante InputHelper
                if (!string.IsNullOrWhiteSpace(emailInput))
                {
                    var normalized = InputHelper.PromptEmail("Correo: ");
                    existing.Email = normalized;
                }

                // Guarda los cambios en el servicio
                _service.UpdatePatient(existing);
                Console.WriteLine("✅ Paciente actualizado.");
            }
            catch (Exception ex)
            {
                // Captura cualquier error en el proceso
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        // ======================================
        // MÉTODO: Listar todos los pacientes
        // ======================================
        public void List()
        {
            Console.WriteLine("=== Lista de Pacientes ===");

            // Recorre todos los pacientes y muestra su información de forma enmascarada
            foreach (var p in _service.GetAll())
            {
                // Mostrar datos sensibles enmascarados
                Console.WriteLine($"Id:{p.Id} | {p.Name} | Doc:{p.MaskDocument()} | Edad:{p.Age} | Tel:{p.MaskPhone()} | Email:{p.Email}");
            }
        }

        // ======================================
        // MÉTODO: Eliminar paciente por documento
        // ======================================
        public void Delete()
        {
            Console.WriteLine("=== Eliminar Paciente ===");

            // Solicita el documento del paciente a eliminar
            var doc = InputHelper.PromptDocument("Documento del paciente a eliminar: ");
            var existing = _service.GetByDocument(doc);

            // Verifica si el paciente existe antes de eliminar
            if (existing == null)
            {
                Console.WriteLine("Paciente no encontrado.");
                return;
            }

            // Elimina el paciente a través del servicio usando su ID
            _service.DeletePatient(existing.Id);
            Console.WriteLine($"Paciente eliminado: {existing.Name} ({existing.MaskDocument()})");
        }
    }
}
