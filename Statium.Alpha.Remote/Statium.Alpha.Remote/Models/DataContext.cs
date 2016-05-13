using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MySql.Data.Entity;

namespace Statium.Alpha.Remote.Models
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DataContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public DataContext() : base("name=DataContext")
        {
        }

        public System.Data.Entity.DbSet<Statium.Alpha.Remote.Models.Method> Methods { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<CustomRole>().HasKey(r => new { r.Id });
            modelBuilder.Entity<CustomUserLogin>().HasKey(l => new {l.UserId, l.ProviderKey, l.LoginProvider});
        }

        public System.Data.Entity.DbSet<Statium.Alpha.Remote.Models.Grid> Grids { get; set; }

        public System.Data.Entity.DbSet<Statium.Alpha.Remote.Models.User> Users { get; set; }
    }
}
