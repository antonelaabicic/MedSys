using MedSys.BL.DALModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedSys.BL.Repositories
{
    public class MedicalHistoryRepository : IMedicalHistoryRepository
    {
        private readonly PostgresContext _context;
        public MedicalHistoryRepository(PostgresContext context)
        {
            _context = context;
        }

        public MedicalHistory Delete(int id)
        {
            var medicalHistory = GetById(id);
            if (medicalHistory == null)
            {
                throw new Exception($"Medical history with id {id} not found.");
            }

            var patientName = medicalHistory.Patient?.FirstName;
            var diseaseName = medicalHistory.Disease?.Name;

            _context.MedicalHistories.Remove(medicalHistory);
            Save();
            return medicalHistory;
        }

        public IEnumerable<MedicalHistory> GetAll()
        {
            return _context.MedicalHistories.ToList();
        }

        public MedicalHistory? GetById(int id)
        {
            return _context.MedicalHistories.FirstOrDefault(p => p.Id == id);
        }

        public MedicalHistory? GetExistingMedicalDocument(int patientId, int diseaseId)
        {
            return _context.MedicalHistories.FirstOrDefault(b => b.PatientId == patientId && b.DiseaseId == diseaseId);
        }

        public IEnumerable<MedicalHistory> GetMedicalHistoryByPatientId(int patientId)
        {
            return _context.MedicalHistories
                .Where(mh => mh.PatientId == patientId)
                .AsNoTracking() 
                .ToList();
        }

        public void Insert(MedicalHistory entity)
        {
            _context.MedicalHistories.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(MedicalHistory entity)
        {
            _context.MedicalHistories.Update(entity);
        }
    }
}
