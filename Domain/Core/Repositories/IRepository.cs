using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IList<T>> GetAllAsync();
        Task<Guid> AddAsync(T entity);
        Task Update(T entity);
        Task Delete(Guid id);
        Task<IEnumerable<T>> GetAllWithIncludes(params string[] includes);
    }
}
