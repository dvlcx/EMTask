using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMTaskApp.DAL
{
    public interface IRepository<T>
    {
        public Task<List<T>> ReadFile();
    }
}