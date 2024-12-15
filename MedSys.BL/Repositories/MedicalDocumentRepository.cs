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
            var medicalDocument = GetById(id);
            if (medicalDocument == null)
            {
                throw new Exception($"Medical document with id {id} not found.");
            }

            var checkupPatientName = medicalDocument.Checkup.Patient?.FirstName;

            _context.MedicalDocuments.Remove(medicalDocument);
            Save();
            return medicalDocument;
        }

        public IEnumerable<MedicalDocument> GetAll()
        {
            return _context.MedicalDocuments.ToList();
        }

        public IEnumerable<MedicalDocument> GetMedicalDocumentByCheckupId(int checkupId)
        {
            return _context.MedicalDocuments.Where(p => p.CheckupId == checkupId).ToList();
        }

        public MedicalDocument? GetById(int id)
        {
            return _context.MedicalDocuments.FirstOrDefault(p => p.Id == id);
        }

        public void Insert(MedicalDocument entity)
        {
            _context.MedicalDocuments.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(MedicalDocument entity)
        {
            _context.MedicalDocuments.Update(entity);
        }

        public IEnumerable<MedicalDocument> GetMedicalDocumentByPatientId(int patientId)
        {
            return _context.MedicalDocuments.Where(p => p.Checkup.PatientId == patientId).ToList();
        }
    }
}