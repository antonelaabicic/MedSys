using MedSys.BL.DALModels;

namespace MedSys.BL.Repositories
{
    public class CheckupRepository : IRepository<Checkup>
    {
        private readonly PostgresContext _context;
        public CheckupRepository(PostgresContext context)
        {
            _context = context;
        }
        public IEnumerable<Checkup> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Checkup? GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(Checkup entity)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Checkup entity)
        {
            throw new System.NotImplementedException();
        }

        public Checkup Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }
    }
}