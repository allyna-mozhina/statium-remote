using System;
using System.Collections.Generic;
using System.Linq;
using Statium.Alpha.Remote.Data.Models;

namespace Statium.Alpha.Remote.Data.Repositories
{
    public class MethodHardcodeRepository : IRepository<Method>
    {
        private static readonly ICollection<Method> Methods = new List<Method>()
        {
            new Method { Id = 1, Name = "logistic_regression" }
        };

        public IQueryable<Method> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Method> GetWhere(Func<Method, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Method Find(int id)
        {
            return Methods.FirstOrDefault(m => m.Id == id);
        }

        public bool Add(Method model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, Method model)
        {
            throw new NotImplementedException();
        }
    }
}