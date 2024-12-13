using MedSys.BL.DALModels;

namespace MedSys.BL.Repositories
{
    public class MedicalDocumentRepository : IMedicalDocumentRepository
    {
        private readonly PostgresContext _context;
        public MedicalDocumentRepository(PostgresContext context)
        {
            _context = context;
        }

        public MedicalDocument Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MedicalDocument> GetAll()
        {
            throw new NotImplementedException();
        }

        public MedicalDocument? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(MedicalDocument entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(MedicalDocument entity)
        {
            throw new NotImplementedException();
        }
    }
}