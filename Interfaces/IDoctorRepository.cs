using System.Collections.Generic;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Interfaces
{
    public interface IDoctorRepository
    {
        void Add(Doctor doctor);
        void Update(Doctor doctor);
        Doctor? GetById(int id);
        Doctor? GetByDocument(string document);
        List<Doctor> GetAll();
        List<Doctor> GetBySpecialty(string specialty);
    }
}
