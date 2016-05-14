using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using Statium.Alpha.Remote.Data.UserMigrations;

namespace Statium.Alpha.Remote.Data.Models
{
    // You can add profile data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name)
        {
            Name = name;
        }
    }

    public class CustomUserStore : UserStore<User, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(UserDbContext context) : base(context)
        {
            
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(UserDbContext context) : base(context)
        {
            
        }
    }

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class UserDbContext : IdentityDbContext<User, CustomRole, 
        int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public UserDbContext()
            : base("StatiumUserConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<UserDbContext, Configuration>("StatiumUserConnection"));
        }
        
        public static UserDbContext Create()
        {
            return new UserDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Ignore(u => u.LockoutEnabled);
            modelBuilder.Entity<User>().Ignore(u => u.LockoutEndDateUtc);
            modelBuilder.Entity<User>().Ignore(u => u.PhoneNumber);
            modelBuilder.Entity<User>().Ignore(u => u.PhoneNumberConfirmed);
            modelBuilder.Entity<User>().Ignore(u => u.TwoFactorEnabled);
            modelBuilder.Entity<User>().Ignore(u => u.AccessFailedCount);

            modelBuilder.Entity<CustomUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<CustomRole>().HasKey(r => new { r.Id });
            modelBuilder.Entity<CustomUserLogin>().HasKey(l => new { l.UserId, l.ProviderKey, l.LoginProvider });
        }
    }
}