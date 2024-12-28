using MedSys.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSys.BL.Repositories
{
    public interface ICheckupRepository : IRepository<Checkup>
    {
        IEnumerable<Checkup> GetCheckupsByPatientId(int patientId);
        Checkup? GetExistingCheckup(int patientId, int checkupTypeId, DateOnly date);
    }
}
