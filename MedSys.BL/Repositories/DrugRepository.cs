using MedSys.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSys.BL.Repositories
{
    public class DrugRepository : IDrugRepository
    {
        private readonly PostgresContext _context;
        public DrugRepository(PostgresContext context)
        {
            _context = context;
        }
        public Drug Delete(int id)
        {
            var drug = GetById(id);

            if (drug == null)
            {
                throw new Exception($"Drug with id {id} not found.");
            }

            _context.Prescriptions.RemoveRange(drug.Prescriptions);
            _context.Drugs.Remove(drug);
            Save();

            return drug;
        }

        public IEnumerable<Drug> GetAll()
        {
            return _context.Drugs.ToList();
        }

        public Drug? GetById(int id)
        {
            return _context.Drugs.FirstOrDefault(b => b.Id == id);
        }

        public Drug? GetDrugByName(string name)
        {
            return _context.Drugs.FirstOrDefault(d => d.Name == name);
        }

        public void Insert(Drug entity)
        {
            _context.Drugs.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Drug entity)
        {
            _context.Drugs.Update(entity);
        }
    }
}
