using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statium.Alpha.Remote.Data.Models;

namespace Statium.Alpha.Remote.Data.Repositories
{
    public class DbRepository<TEntity, TContext> : IRepository<TEntity> 
        where TEntity : class 
        where TContext : DbContext, new() 
    {
        private readonly TContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public DbRepository()
        {
            _dbContext = new TContext(); 
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual IEnumerable<TEntity> GetWhere(Func<TEntity, bool> predicate)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
            {
                return query.Where(predicate);
            }

            return query.ToList();
        }

        public virtual TEntity Find(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual bool Add(TEntity entity)
        {
            return _dbSet.Add(entity) != null;
        }

        public virtual bool Delete(int id)
        {
            TEntity entity = Find(id);

            return Delete(entity);
        }

        public virtual bool Delete(TEntity entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            return _dbSet.Remove(entityToDelete) != null;
        }

        public bool Update(TEntity entityToUpdate)
        {
            var result = _dbSet.Attach(entityToUpdate) != null;

            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;

            return result;
        }

        public virtual void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
