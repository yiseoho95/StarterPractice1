using ApplicationCore.Models.Response;
using ApplicationCore.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EfRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly StarterPracticeDbContext _dbContext;
        public EfRepository(StarterPracticeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            //Example Movie - Title, Revenue, Budget
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
            {
                return await _dbContext.Set<T>().Where(filter).CountAsync();
            }
            return await _dbContext.Set<T>().CountAsync();
        }

        public async Task<bool> GetExistAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
            {
                return false;
            }
            return await _dbContext.Set<T>().Where(filter).AnyAsync();

        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {

            return await _dbContext.Set<T>().ToListAsync();
        }

        public Task<IEnumerable<T>> ListAllWithIncludesAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }


    }
}
