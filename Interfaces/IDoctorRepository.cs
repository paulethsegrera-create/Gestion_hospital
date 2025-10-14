using System.Collections.Generic;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Interfaces
{
    public interface IDoctorRepository
    {
    void Add(Doctor doctor);
    void Update(Doctor doctor);
    void Delete(int id);
    Doctor? GetById(int id);
    Doctor? GetByDocument(string document);
    List<Doctor> GetAll();
    List<Doctor> GetBySpecialty(string specialty);
    }
}
