using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statium.Alpha.Remote.Models;

namespace Statium.Alpha.Remote.Repositories
{
    interface IRepository<TModel>
    {
        IQueryable<TModel> GetAll();

        IQueryable<TModel> GetWhere(Func<TModel, bool> predicate);

        TModel Find(int id);

        bool Add(TModel model);

        bool Delete(int id);

        bool Update(int id, TModel model);
    }
}
