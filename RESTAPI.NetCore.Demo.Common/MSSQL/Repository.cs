using Microsoft.EntityFrameworkCore;
using RESTAPI.NetCore.Demo.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RESTAPI.NetCore.Demo.Common.MSSQL
{
    public class Repository<T> : IRepository<T>
        where T : class, IEntity
    {
        private readonly DataContext _dataContext;

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, int skip, int take)
        {
            return await _dataContext.Set<T>().AsQueryable().Where(predicate).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public async Task Add(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _dataContext.Set<T>().Add(entity);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }
        }

        public async Task BulkAdd(List<T> list)
        {
            try
            {
                foreach (var item in list)
                {
                    _dataContext.Set<T>().Add(item);
                }

                await _dataContext.SaveChangesAsync();
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }
        }

        public async Task Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _dataContext.Set<T>().Update(entity);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }
        }

        public async Task Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _dataContext.Set<T>().Remove(entity);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }
        }
    }
}
