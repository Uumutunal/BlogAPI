using Domain.Core.Models;
using Domain.Core.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        public AppDbContext _context { get; }
        private readonly DbSet<T> _entities;
        private readonly IUnitOfWork _unitOfWork;

        public Repository(AppDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _entities = _context.Set<T>();
        }


        public async Task<Guid> AddAsync(T entity)
        {
            entity.CreatedDate = DateTime.Now;
            await _entities.AddAsync(entity);
            //_context.SaveChanges();
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.DeletedDate = DateTime.Now;
                _entities.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _entities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task Update(T entity)
        {
            entity.ModifiedDate = DateTime.Now;

            if (_entities.Local.All(e => e != entity))
            {
                _entities.Attach(entity);
            }

            _entities.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _entities.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithIncludes(params string[] includes)
        {
            IQueryable<T> query = _entities;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.Where(x => !x.IsDeleted).ToListAsync();
        }
    }
}
