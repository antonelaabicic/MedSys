using MedSys.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSys.BL.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Patient? FindPatientByDetails(string firstName, string lastName, DateTime dateOfBirth);
        Patient? FindPatientByLastName(string lastName);
        Patient? FindPatientByOIB(string oib);
    }
}
