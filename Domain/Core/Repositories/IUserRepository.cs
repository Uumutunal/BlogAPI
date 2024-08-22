using Domain.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Repositories
{
    public interface IUserRepository<T> where T : IdentityUser
    {
        Task<T> GetByIdAsync(string id);
        Task<IList<T>> GetAllAsync();
        Task<string> AddAsync(T entity);
        Task Update(T entity);
        Task Delete(string id);
    }
}
