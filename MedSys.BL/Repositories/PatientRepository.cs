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
            var patient = GetById(id);
            if (patient == null)
            {
                throw new Exception($"Patient with id {id} not found.");
            }

            _context.Prescriptions.RemoveRange(patient.Prescriptions);
            _context.Checkups.RemoveRange(patient.Checkups);
            _context.MedicalHistories.RemoveRange(patient.MedicalHistories);

            _context.Patients.Remove(patient);
            Save();
            return patient;
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
            return _context.Patients.ToList();
        }

        public Patient? GetById(int id)
        {
            return _context.Patients.FirstOrDefault(p => p.Id == id);
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
