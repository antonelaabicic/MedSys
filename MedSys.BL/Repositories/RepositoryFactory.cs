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

        public T GetRepository<T>() where T : class
        {
            var repository = _serviceProvider.GetService<T>() ?? 
                throw new InvalidOperationException($"Repository of type {typeof(T).FullName} is not registered.");

            return repository;
        }
    }
}
