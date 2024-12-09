using MedSys.BL.DALModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSys.BL.Repositories
{
    public class CheckupTypeRepository : IRepository<CheckupType>
    {
        private readonly PostgresContext _context;
        public CheckupTypeRepository(PostgresContext context)
        {
            _context = context;
        }

        public CheckupType Delete(int id)
        {
            var checkupType = GetById(id);

            if (checkupType == null)
            {
                throw new Exception($"Checkup type with id {id} not found.");
            }

            _context.Checkups.RemoveRange(checkupType.Checkups);
            _context.CheckupTypes.Remove(checkupType);

            Save();

            return checkupType;
        }

        public IEnumerable<CheckupType> GetAll()
        {
            return _context.CheckupTypes.ToList();
        }

        public CheckupType? GetById(int id)
        {
            return _context.CheckupTypes.FirstOrDefault(b => b.Id == id);
        }

        public void Insert(CheckupType entity)
        {
            _context.CheckupTypes.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(CheckupType entity)
        {
            _context.CheckupTypes.Update(entity);
        }
    }
}
