using MedSys.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSys.BL.Repositories
{
    public interface IMedicalHistoryRepository : IRepository<MedicalHistory>
    {
        IEnumerable<MedicalHistory> GetMedicalHistoryByPatientId(int patientId);
        MedicalHistory? GetExistingMedicalDocument(int patientId, int diseaseId);
    }
}
