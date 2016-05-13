using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.Identity;
using Statium.Alpha.Remote.Models;

namespace Statium.Alpha.Remote.Repositories
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