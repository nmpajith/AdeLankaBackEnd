using AdeLankaBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null
            ,Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
            , PageParameters pageParameters= null
            ,params Expression<Func<TEntity, object>>[] including);
        TEntity GetByID(object id);
        void Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
    }
}
