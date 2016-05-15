using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statium.Alpha.Remote.Data.Repositories
{
    //Assumes TEntity has an int property named "Id"
    public class HardcodeRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ICollection<TEntity> _entities;

        public HardcodeRepository(ICollection<TEntity> entities)
        {
            _entities = entities;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entities.ToList();
        }

        public IEnumerable<TEntity> GetWhere(Func<TEntity, bool> predicate)
        {
            if (predicate != null)
            {
                return _entities.Where(predicate);
            }

            return _entities.ToList();
        }

        public TEntity Find(int id)
        {
            return _entities.FirstOrDefault(e => (int)e.GetType().GetProperty("Id").GetValue(e) == id);
        }

        //Assigns entity.Id collection.Count value and adds entity to collection
        public bool Add(TEntity entity)
        {
            if (entity != null)
            {
                entity.GetType().GetProperty("Id")?.SetValue(entity, _entities.Count);
                _entities.Add(entity);
                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            var entity = Find(id);
            return Delete(entity);
        }

        public bool Delete(TEntity entityToDelete)
        {
            return _entities.Remove(entityToDelete);
        }

        public bool Update(TEntity entityToUpdate)
        {
            var id = (int) entityToUpdate.GetType().GetProperty("Id").GetValue(entityToUpdate);
            Delete(id);
            return Add(entityToUpdate);
        }

        public void Dispose()
        {
            
        }
    }
}
