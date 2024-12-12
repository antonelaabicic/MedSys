using MedSys.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSys.BL.Repositories
{
    public class DiseaseRepository : IRepository<Disease>
    {
        private readonly PostgresContext _context;
        public DiseaseRepository(PostgresContext context)
        {
            _context = context;
        }
        public Disease Delete(int id)
        {
            var disease = GetById(id);

            if (disease == null)
            {
                throw new Exception($"Disease with id {id} not found.");
            }

            _context.MedicalHistories.RemoveRange(disease.MedicalHistories);
            _context.Diseases.Remove(disease);
            Save();

            return disease;
        }

        public IEnumerable<Disease> GetAll()
        {
            return _context.Diseases.ToList();
        }

        public Disease? GetById(int id)
        {
            return _context.Diseases.FirstOrDefault(b => b.Id == id);
        }

        public void Insert(Disease entity)
        {
            _context.Diseases.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Disease entity)
        {
            _context.Diseases.Update(entity);
        }
    }
}
