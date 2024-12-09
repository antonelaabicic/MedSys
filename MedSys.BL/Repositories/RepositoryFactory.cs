using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSys.BL.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return _serviceProvider.GetService<IRepository<T>>() ?? 
                throw new InvalidOperationException($"No such repository exists.");
        }
    }
}
