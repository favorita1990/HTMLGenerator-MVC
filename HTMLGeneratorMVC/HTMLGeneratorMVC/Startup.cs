using System;
using System.Linq;
using HTMLGeneratorMVC.Context;
using HTMLGeneratorMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HTMLGeneratorMVC.Startup))]
namespace HTMLGeneratorMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }
        private void createRolesandUsers()
        {
            GeneratorDb context = new GeneratorDb();
            var roleManager = new RoleManager<Identity2Role, int>(new RoleStore(context));
            var userManager = new UserManager<UserModel, int>(new UserStore(context));

            var role = "Admin";
            var email = "founder@abv.bg";
            var pass = "12";
            if (!roleManager.RoleExists(role))
            {
                roleManager.Create(new Identity2Role(role));
            }

            var PasswordHash = new PasswordHasher();
            if (!context.Users.Any(u => u.Email == email))
            {
                var user = new UserModel
                {
                    Email = email,
                    UserName = email,
                    FirstName = "Iliyan",
                    LastName = "Kodzhemanov",
                    Genter = "1",
                    Time = DateTime.Now,
                    PasswordHash = PasswordHash.HashPassword(pass)
                };

                userManager.Create(user);
                userManager.AddToRole(user.Id, role);

                // creating Creating User role    
                if (!roleManager.RoleExists("User"))
                {
                    roleManager.Create(new Identity2Role("User"));
                }
            }
        }
    }
}
