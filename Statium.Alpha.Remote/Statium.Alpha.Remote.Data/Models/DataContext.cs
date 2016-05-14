using System.Data.Entity;

namespace Statium.Alpha.Remote.Data.Models
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

        public System.Data.Entity.DbSet<Method> Methods { get; set; }
        
        public System.Data.Entity.DbSet<Grid> Grids { get; set; }
    }
}
