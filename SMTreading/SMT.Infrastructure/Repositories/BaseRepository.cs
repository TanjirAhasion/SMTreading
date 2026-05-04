using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        private IDbContextTransaction? _transaction;
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
        public async Task CreateRangeAsync(IEnumerable<T> entities)
        {
            await Db.Set<T>().AddRangeAsync(entities);
            await Db.SaveChangesAsync();
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

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await Db.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await Db.SaveChangesAsync() > 0;
        }
    }
}
