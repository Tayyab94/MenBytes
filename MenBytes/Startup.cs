using MenBytes.Models;
using MenBytes.Models.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MenBytes.Startup))]
namespace MenBytes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR();
            CreateUser();
        }

        protected void CreateUser()
        {
            ApplicationDbContext _context= new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

            var userManager=new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

            if (!roleManager.RoleExists(MB.Admin))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = MB.Admin;

                roleManager.Create(role);

                var user= new ApplicationUser();

                user.UserName = "admin_mb@gmail.com";
                user.Email = "admin_mb@gmail.com";

                user.user_Name = "Admin";
                user.accountPass = "Admin!23";
                string userPass = "Admin!23";
                var checkUser = userManager.Create(user, userPass);
                if (checkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, MB.Admin);
                }
            }

            if (!roleManager.RoleExists(MB.User))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = MB.User;

                roleManager.Create(role);
            }
        }
    }
}
