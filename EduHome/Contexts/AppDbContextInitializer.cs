using EduHome.Identity;
using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Identity;

namespace EduHome.Contexts
{
    public class AppDbContextInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public AppDbContextInitializer(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task UserSeedAsync()
        {
            foreach (var role in Enum.GetValues(typeof(Roles))  )  {


                await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
            }


            AppUser adminUser = new AppUser
            {
                UserName = "admin",
                Email = "cananovalaman@gmail.com",
                FullName = "adminadmin",
                IsActive = true,
            };
            await _userManager.CreateAsync(adminUser, "Salam123!");
            await _userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
        } }
}
