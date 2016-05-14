using System;
using System.Linq;
using Statium.Alpha.Remote.Data.Models;

namespace Statium.Alpha.Remote.Data.Repositories
{
    public class MethodDbRepository : IRepository<Method>, IDisposable
    {
        private readonly DataContext _db = new DataContext();

        public IQueryable<Method> GetAll()
        {
            return _db.Methods;
        }

        public IQueryable<Method> GetWhere(Func<Method, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Method Find(int id)
        {
            throw new NotImplementedException();
        }

        public bool Add(Method model)
        {
            throw new NotImplementedException();
            //await _db.SaveChangesAsync();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, Method model)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            ((IDisposable) _db).Dispose();
        }
    }
}