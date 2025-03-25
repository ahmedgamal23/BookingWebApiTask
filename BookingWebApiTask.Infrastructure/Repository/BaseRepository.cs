using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookingWebApiTask.Application.Interfaces;
using BookingWebApiTask.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingWebApiTask.Infrastructure.Repository
{
    public class BaseRepository<T, TType>:IBaseRepository<T, TType> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? include = null, Expression<Func<T, bool>>? filter = null, int pageNumber = 1, int pageSize = 5)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
                query = include(query);

            if (filter != null)
                query = query.Where(filter);

            return await query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<T?> GetAsync(TType id, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
                query = include(query);

            var entity = await query.FirstOrDefaultAsync(e => EF.Property<TType>(e, "Id")!.Equals(id));

            //if (entity == null)
            //    throw new KeyNotFoundException("Entity not found");

            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        public Task<T> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.FromResult(entity);
        }
    }
}

