using System.Data.Entity.Migrations;
using NajmDefault.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace NajmDefault.migrations
{
    internal sealed class configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //context.Roles.AddOrUpdate(r => r.Name,
            //    new IdentityRole { Name = "Admin" },
            //    new IdentityRole { Name = "Supervisor" }
            //    );

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string[] rolenames = { "Admin", "Supervisor" };
            IdentityResult roleresult;
            foreach (var rolename in rolenames)
            {
                if (!RoleManager.RoleExists(rolename))
                {
                    roleresult = RoleManager.Create(new IdentityRole(rolename));
                }
            }
        }
    }
}