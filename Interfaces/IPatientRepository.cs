using System.Collections.Generic;
using Gestion_Hospital.Models;

namespace Gestion_Hospital.Interfaces
{
    public interface IPatientRepository
    {
    void Add(Patient patient);
    void Update(Patient patient);
    void Delete(int id);
    Patient? GetById(int id);
    Patient? GetByDocument(string document);
    List<Patient> GetAll();
    }
}
