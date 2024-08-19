using AdeLankaBackEnd.Contracts;
using AdeLankaBackEnd.Domain.Models;
using AdeLankaBackEnd.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.Persistence.Repositories
{
    public class GenericRepository<TEntity> :IGenericRepository<TEntity> where TEntity:class
    {
        internal AppDbContext _context;
        internal DbSet<TEntity> _dbSet;
        ILoggerManager _logger;

        public GenericRepository(AppDbContext context, ILoggerManager logger)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _logger = logger;
        }

        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", PageParameters pageParameters = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (pageParameters != null)
            {
                if (orderBy != null)
                {
                    return orderBy(query)
                        .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                        .Take(pageParameters.PageSize)
                        .ToList();
                }
                else
                {
                    return query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                        .Take(pageParameters.PageSize)
                        .ToList();
                }
            }
            else
            {

                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                else
                {
                    return query.ToList();
                }
            }
        }

        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null
            ,Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
            , PageParameters pageParameters = null,
            params Expression<Func<TEntity, object>>[] including)
        {
            IQueryable<TEntity> query = _dbSet;
            
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (including != null)
                including.ToList().ForEach(include =>
                {
                    _logger.LogInfo("Incuding:" + include.ToString());
                    if (include != null)
                        query = query.Include(include);
                }
                );

            if (pageParameters != null)
            {
                if (orderBy != null)
                {
                    return orderBy(query)
                        .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                        .Take(pageParameters.PageSize)
                        .ToList();
                }
                else
                {
                    return query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                        .Take(pageParameters.PageSize)
                        .ToList();
                }
            }
            else
            {

                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                else
                {
                    return query.ToList();
                }
            }
        }

        public TEntity GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
