﻿using MedSys.BL.DALModels;
using Microsoft.EntityFrameworkCore;

namespace MedSys.BL.Repositories
{
    public class CheckupRepository : ICheckupRepository
    {
        private readonly PostgresContext _context;
        public CheckupRepository(PostgresContext context)
        {
            _context = context;
        }
        public IEnumerable<Checkup> GetAll()
        {
            return _context.Checkups.ToList();
        }

        public Checkup? GetById(int id)
        {
            return _context.Checkups
                .Include(c => c.Patient)
                .Include(c => c.CheckupType)
                .FirstOrDefault(c => c.Id == id);
        }

        public void Insert(Checkup entity)
        {
            _context.Checkups.Add(entity);
        }

        public void Update(Checkup entity)
        {
            _context.Checkups.Update(entity);
        }

        public Checkup Delete(int id)
        {
            var checkup = GetById(id);

            if (checkup == null)
            {
                throw new Exception($"Checkup with id {id} not found.");
            }

            _context.MedicalDocuments.RemoveRange(checkup.MedicalDocuments);
            _context.Checkups.Remove(checkup);

            Save();

            return checkup;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Checkup> GetAllByPatientId(int patientId)
        {
            return _context.Checkups
                .Include(c => c.Patient)
                .Include(c => c.CheckupType)
                .Where(c => c.PatientId == patientId)
                .ToList();
        }

        public Checkup? GetExistingCheckup(int patientId, int checkupTypeId, DateOnly date, TimeOnly time)
        {
            return _context.Checkups
                    .FirstOrDefault(b => b.PatientId == patientId && b.CheckupTypeId == checkupTypeId && b.Date == date && b.Time == time);
        }
    }
}