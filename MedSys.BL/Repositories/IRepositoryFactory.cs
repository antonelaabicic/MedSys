using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSys.BL.Repositories
{
    public interface IRepositoryFactory
    {
        IRepository<T> GetRepository<T>() where T : class;
    }

}
