using System;
using System.Linq;

namespace Statium.Alpha.Remote.Data.Repositories
{
    public interface IRepository<TModel>
    {
        IQueryable<TModel> GetAll();

        IQueryable<TModel> GetWhere(Func<TModel, bool> predicate);

        TModel Find(int id);

        bool Add(TModel model);

        bool Delete(int id);

        bool Update(int id, TModel model);
    }
}
