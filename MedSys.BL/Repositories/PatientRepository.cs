using MedSys.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSys.BL.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PostgresContext _context;
        public PatientRepository(PostgresContext context)
        {
            _context = context;
        }
        public Patient Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Patient? FindPatientByDetails(string firstName, string lastName, DateTime dateOfBirth)
        {
            return _context.Patients
                .FirstOrDefault(p => p.FirstName == firstName &&
                                     p.LastName == lastName &&
                                     p.DateOfBirth == DateOnly.FromDateTime(dateOfBirth));
        }

        public Patient? FindPatientByLastName(string lastName)
        {
            return _context.Patients.FirstOrDefault(p => p.LastName == lastName);
        }

        public Patient? FindPatientByOIB(string oib)
        {
            return _context.Patients.FirstOrDefault(p => p.Oib == oib);
        }

        public IEnumerable<Patient> GetAll()
        {
            throw new NotImplementedException();
        }

        public Patient? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Patient entity)
        {
            _context.Patients.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Patient entity)
        {
            _context.Patients.Update(entity);
        }


    }
}
