using MedSys.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedSys.BL.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly PostgresContext _context;

        public PrescriptionRepository(PostgresContext context)
        {
            _context = context;
        }
        public Prescription Delete(int id)
        {
            var prescription = GetById(id);
            if (prescription == null)
            {
                throw new Exception($"Prescription with id {id} not found.");
            }

            var patientName = prescription.Patient?.FirstName; 
            var drugName = prescription.Drug?.Name;

            _context.Prescriptions.Remove(prescription);
            Save();
            return prescription;
        }

        public IEnumerable<Prescription> GetAll()
        {
            return _context.Prescriptions.ToList();
        }

        public Prescription? GetById(int id)
        {
            return _context.Prescriptions.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Prescription> GetByPatientId(int patientId)
        {
            return _context.Prescriptions.Where(p => p.PatientId == patientId).ToList();
        }

        public Prescription? GetExistingPrescription(int patientId, int drugId, DateTime issueDate)
        {
            return _context.Prescriptions.FirstOrDefault(p => p.PatientId == patientId &&
                p.DrugId == drugId && p.IssueDate == DateOnly.FromDateTime(issueDate));
        }

        public void Insert(Prescription entity)
        {
            _context.Prescriptions.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Prescription entity)
        {
            _context.Prescriptions.Update(entity);
        }
    }
}
