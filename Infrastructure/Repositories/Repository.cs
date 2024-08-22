using Domain.Core.Models;
using Domain.Core.Repositories;
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

        public async Task<IList<T>> GetAllAsync()
        {
            return await _entities.Where(x => !x.IsDeleted).ToListAsync();

        }

        public void Update(T entity)
        {
            _unitOfWork.SaveChangesAsync();
        }
    }
}
