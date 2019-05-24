using Microsoft.EntityFrameworkCore;
using POSM.Data;
using POSM.Services.Interfact;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSM.Services.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public BaseService(ApplicationDbContext context)
        {
            _context = context;
        }
        private DbSet<T> GetDbContext<T>() where T : class
        {
            return _context.Set<T>();
        }

        public async Task DeleteManyAsync<T>(List<T> entities) where T : class
        {
            GetDbContext<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            GetDbContext<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task InsertAsync<T>(T entity) where T : class
        {
            GetDbContext<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task InsertManyAsync<T>(List<T> entities) where T : class
        {
            GetDbContext<T>().AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            GetDbContext<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateManyAsync<T>(List<T> entities) where T : class
        {
            GetDbContext<T>().UpdateRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
