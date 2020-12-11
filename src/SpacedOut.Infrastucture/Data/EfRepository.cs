using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SpacedOut.SharedKernal.Interfaces;
using SpacedOut.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpacedOut.Infrastucture.Data
{
    public class EfRepository : IRepository, IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? Transaction { get; init; }

        public EfRepository(AppDbContext context)
        {
            _context = context;
        }

        public static EfRepository CreateWithTransaction(AppDbContext context)
        {
            return new EfRepository(context)
            {
                Transaction = context.Database.BeginTransaction()
            };
        }

        public Task<T> GetByIdAsync<T>(int id) where T : BaseEntity
        {
            return _context.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
        }

        public Task<List<T>> ListAsync<T>() where T : BaseEntity
        {
            return _context.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync<T>(T entity) where T : BaseEntity
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync<T>(T entity) where T : BaseEntity
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : BaseEntity
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Task CommitAsync()
        {
            return Transaction?.CommitAsync() ?? Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            return Transaction?.RollbackAsync() ?? Task.CompletedTask;
        }

        public void Dispose()
        {
            Transaction?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}