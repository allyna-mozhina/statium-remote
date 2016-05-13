using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Statium.Alpha.Remote.Models;

namespace Statium.Alpha.Remote.Repositories
{
    public class GridHardcodeRepository : IRepository<Grid>
    {
        private static readonly ICollection<Grid> Grids = new List<Grid>()
        {
            new Grid() { Id = 1, Name = "Patients", DomainName = "Global" }
        };
        
        public IQueryable<Grid> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Grid> GetWhere(Func<Grid, bool> predicate)
        {
            return Grids.Where(predicate).AsQueryable();
        }

        public Grid Find(int id)
        {
            return Grids.FirstOrDefault(g => g.Id == id);
        }

        public bool Add(Grid grid)
        {
            if (grid != null)
            {
                grid.Id = Grids.Count;
                Grids.Add(grid);
                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, Grid grid)
        {
            throw new NotImplementedException();
        }
    }
}