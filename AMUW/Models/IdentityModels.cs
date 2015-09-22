using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AMUW.Models
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
        public class DropCreateAlwaysInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {
                ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                var rm = new RoleManager<IdentityRole>(
                     new RoleStore<IdentityRole>(context));
                var adminRole = rm.Create(new IdentityRole("Administrator"));
                var userRole = rm.Create(new IdentityRole("User"));

                ApplicationUser user = new ApplicationUser();
                PasswordHasher passwordHasher = new PasswordHasher();

                user.UserName = "admin@amuw.com";
                user.Email = "admin@amuw.com";
                user.Id = "da7ae7ea-be5b-4c12-98bc-7c78d4665ed0";

                IdentityResult result = userManager.Create(user, "Amuw2015!");

                userManager.AddToRole(user.Id, "Administrator");
                base.Seed(context);
            }
        }
    }
}