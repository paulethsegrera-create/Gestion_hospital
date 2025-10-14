using System.Collections.Generic; // Importa la colección genérica List<T> y otras colecciones
using Gestion_Hospital.Models; // Importa el espacio de nombres donde se encuentra la clase Doctor

namespace Gestion_Hospital.Interfaces // Define el namespace para organizar el código de interfaces del hospital
{
    // Define una interfaz para el repositorio de Doctores
    public interface IDoctorRepository
    {
        void Add(Doctor doctor); // Método para agregar un nuevo doctor
        void Update(Doctor doctor); // Método para actualizar los datos de un doctor existente
        void Delete(int id); // Método para eliminar un doctor mediante su ID
        Doctor? GetById(int id); // Método para obtener un doctor por su ID, puede retornar null si no existe
        Doctor? GetByDocument(string document); // Método para obtener un doctor por su documento, puede retornar null
        List<Doctor> GetAll(); // Método para obtener la lista completa de doctores
        List<Doctor> GetBySpecialty(string specialty); // Método para obtener doctores filtrados por especialidad
    }
}
