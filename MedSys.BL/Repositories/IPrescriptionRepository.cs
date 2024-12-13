using MedSys.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSys.BL.Repositories
{
    public interface IPrescriptionRepository : IRepository<Prescription>
    {
        IEnumerable<Prescription> GetPrescriptionByPatientId(int patientId);
        Prescription? GetExistingPrescription(int patientId, int drugId, DateTime issueDate);
    }
}
