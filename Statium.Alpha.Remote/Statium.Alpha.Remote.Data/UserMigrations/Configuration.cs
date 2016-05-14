using Microsoft.AspNet.Identity;
using Statium.Alpha.Remote.Data.Models;

namespace Statium.Alpha.Remote.Data.UserMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Statium.Alpha.Remote.Data.Models.UserDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"UserMigrations";
        }

        protected override void Seed(Statium.Alpha.Remote.Data.Models.UserDbContext context)
        {
            InitializeUserData(context);

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }

        private async void InitializeUserData(UserDbContext context)
        {
            const string adminRole = "Administrator";
            const string clientRole = "Subscriber";

            var roles = new[] { adminRole, clientRole };
            var roleStore = new CustomRoleStore(context);
            var userStore = new CustomUserStore(context);

            foreach (var role in roles)
            {
                if (!roleStore.Roles.Any(r => r.Name.Equals(role)))
                {
                    await roleStore.CreateAsync(new CustomRole(role));
                }
            }

            var admins = new[]
            {
                new User
                {
                    Email = "mohinalina@gmail.com",
                    UserName = "alina",
                    SecurityStamp = Guid.NewGuid().ToString()
                }/*,
                new User
                {
                    Email = "",
                    UserName = "pavel"
                }*/
            };

            var passwordHasher = new PasswordHasher();

            foreach (var admin in admins)
            {
                if (!userStore.Users.Any(u => u.Email.Equals(admin.Email)))
                {
                    var hashedPassword = passwordHasher.HashPassword("Fit12201");
                    admin.PasswordHash = hashedPassword;

                    await userStore.CreateAsync(admin);
                    await userStore.AddToRoleAsync(admin, adminRole);
                }
            }

            context.SaveChanges();
        }

    }
}
