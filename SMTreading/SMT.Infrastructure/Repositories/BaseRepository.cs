using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Infrastructure.context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SMT.Infrastructure.Services
{
    public class BaseRepository<T>(AppDbContext db) : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext Db = db;

        public async Task<T?> GetByIdAsync(Guid id) =>
            await Db.Set<T>().FindAsync(id);

        public async Task<T?> GetByIdAsync(long id) =>
           await Db.Set<T>().FindAsync(id);

        public async Task<List<T>> GetAllAsync() =>
            await Db.Set<T>().ToListAsync();

        public async Task<T> CreateAsync(T entity)
        {
            Db.Set<T>().Add(entity);
            await Db.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            Db.Set<T>().Update(entity);
            await Db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is null) return false;
            Db.Set<T>().Remove(entity);
            await Db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is null) return false;
            Db.Set<T>().Remove(entity);
            await Db.SaveChangesAsync();
            return true;
        }

    }
}
