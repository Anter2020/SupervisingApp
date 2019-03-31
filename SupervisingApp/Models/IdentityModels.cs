using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NajmDefault.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }

    
    //public class Users : IdentityUser
    //    {
    //    public virtual Users_Info MyUserInfo { get; set; }
    //    }
    //public class Users_Info
    //{
    //    public int name { get; set; }
    //    public string password { get; set; }
    //    public string Tel { get; set; }
    //}
    //public class MyDbContext : IdentityDbContext<Users>
    //{
    //    public MyDbContext()
    //        : base("DefaultConnection")
    //    {
    //    }
    //    public System.Data.Entity.DbSet<Users_Info> MyUserInfo { get; set; }
    // }

}