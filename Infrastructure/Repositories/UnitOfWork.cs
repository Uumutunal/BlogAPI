using Domain.Core.Models;
using Domain.Core.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
        }
        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            throw new NotImplementedException();
        }

        public Task RollBackChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();

        }
    }
}
