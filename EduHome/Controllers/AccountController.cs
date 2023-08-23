using System.Runtime.CompilerServices;
using AutoMapper;
using EduHome.Identity;
using EduHome.Services.Interfaces;
using EduHome.Utils.Enums;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;


        public AccountController(IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMailService mailService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _mailService = mailService;

        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest("You are already registered.");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            if (User.Identity.IsAuthenticated)
            {
                return BadRequest("You are already registered.");
            }
            AppUser newUser = _mapper.Map<AppUser>(registerViewModel);
            newUser.IsActive = false;

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var link = Url.Action("ConfirmEmail", "Account", new { email = registerViewModel.Email, token = token }, HttpContext.Request.Scheme);

            MailRequest mailRequest = new MailRequest
            {
                ToEmail = registerViewModel.Email,
                Subject = "confirmation",
                Body = $"<a href='{link}'>confirm email</a>"
            };
            await _mailService.SendEmailAsync(mailRequest);


            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
            return RedirectToAction("Confirm", "Account");
        }
        public async Task<IActionResult> Confirm()
        {
            return View();
        }
     
        public async Task<IActionResult> ConfirmEmail(String email , string token)
        {
            if (email == null || token == null)
            {
                return NotFound();
            }
                var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
              
                ModelState.AddModelError("", "notfoudn");
                return View();
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "notfoudn");
                return View("Error"); 
            }
            user.IsActive = true;
            await _userManager.UpdateAsync(user);
            return View();
        }
      
    }
}