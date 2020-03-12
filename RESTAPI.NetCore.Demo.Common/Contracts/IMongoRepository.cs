using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RESTAPI.NetCore.Demo.Common.Contracts
{
    public interface IMongoRepository<T>
        where T : IEntity
    {
        IMongoQueryable<T> Get(Expression<Func<T, bool>> predicate);
    }
}
