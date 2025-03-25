using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApiTask.Application.Interfaces
{
    public interface IBaseRepository<T, TType> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync(
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Expression<Func<T, bool>>? filter = null,
            int pageNumber = 1,
            int pageSize = 5
        );

        public Task<T?> GetAsync(
            TType id,
            Func<IQueryable<T>, IQueryable<T>>? include = null
        );

        public Task<T> AddAsync(T entity);

        public T Update(T entity);

        public Task<T> DeleteAsync(T entity);
    }
}
