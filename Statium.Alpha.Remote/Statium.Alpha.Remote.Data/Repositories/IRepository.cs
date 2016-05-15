using System;
using System.Collections.Generic;
using System.Linq;

namespace Statium.Alpha.Remote.Data.Repositories
{
    public interface IRepository<TEntity> : IDisposable
    {
        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetWhere(Func<TEntity, bool> predicate);

        TEntity Find(int id);

        bool Add(TEntity entity);

        bool Delete(int id);

        bool Delete(TEntity entityToDelete);

        bool Update(TEntity entityToUpdate);
    }
}
