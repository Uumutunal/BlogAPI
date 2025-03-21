﻿using Domain.Core.Models;
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
    public class UserRepository<T> : IUserRepository<T> where T : IdentityUser, IAuditableEntity
    {
        public AppDbContext _context { get; }
        private readonly DbSet<T> _entities;
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(AppDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _entities = _context.Set<T>();
        }


        public async Task<string> AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(string id)
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

        public async Task<T> GetByIdAsync(string id)
        {
            return await _entities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _entities.Where(x => !x.IsDeleted).ToListAsync();

        }

        public async Task Update(T entity)
        {
            _context.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
