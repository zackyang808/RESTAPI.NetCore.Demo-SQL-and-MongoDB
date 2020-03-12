using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RESTAPI.NetCore.Demo.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RESTAPI.NetCore.Demo.Common.MongoDB
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly IMongoDatabase _database;
        private IMongoCollection<T> _collection => _database.GetCollection<T>(typeof(T).Name);

        public Repository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, int skip, int take)
        {
            return await _collection.AsQueryable().Where(predicate).ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Add(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                await _collection.InsertOneAsync(entity);
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
                await _collection.InsertManyAsync(list);
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
                await _collection.ReplaceOneAsync(w => w.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
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

                await _collection.DeleteOneAsync(w => w.Id == entity.Id);
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }

        }
    }
}
