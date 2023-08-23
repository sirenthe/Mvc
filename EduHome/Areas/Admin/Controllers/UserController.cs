using AutoMapper;
using EduHome.Areas.Admin.ViewModels;
using EduHome.Contexts;
using EduHome.Identity;
using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
            private readonly UserManager<AppUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;


            public UserController(IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager ,AppDbContext context)
            {
                _mapper = mapper;
                _userManager = userManager;

                _roleManager = roleManager;
            _context = context;
            }
        [Authorize(Roles="Admin, Moderator")]

        public async Task<IActionResult> Index()

        {
            List<AppUser> users = await _context.Users.ToListAsync();
            //  var userViewModel=_mapper.Map<List<UserViewModel>>(users);  
            return View(users);
        }





        public IActionResult ChangeStatus(string id)
        {

            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null)
            {
                return NotFound();
            }


            user.IsActive = !user.IsActive;
            _userManager.UpdateAsync(user).Wait();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangeRole(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            RoleChangeViewModel test=new RoleChangeViewModel()
            {
                USer=user,
                Roles = userRoles
            };
            var roles = Enum.GetValues(typeof(Roles)).Cast<Roles>().ToList();
            ViewBag.Roles = roles;
            
            ViewBag.userRoles = userRoles;
            return View(test);
        }








        [HttpPost]
        public async Task<IActionResult> UpdateRole(string id, Roles newRole)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var currentRole = currentRoles.FirstOrDefault();

            if (currentRole != null)
            {
                await _userManager.RemoveFromRoleAsync(user, currentRole);
            }

            await _userManager.AddToRoleAsync(user, newRole.ToString());

            return RedirectToAction("Index");
        }

    }
}
