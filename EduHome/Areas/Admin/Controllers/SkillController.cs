using AutoMapper;
using EduHome.Areas.Admin.ViewModels.BlogViewModel;
using EduHome.Areas.Admin.ViewModels.SkillViewModel;
using EduHome.Areas.Admin.ViewModels.SliderViewModels;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.Utils.Enums;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static EduHome.ViewModels.DetailTeacherViewModel;

namespace EduHome.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SkillController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SkillController(AppDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
         
        }

        public async Task<IActionResult> Index()
        {

            var skill = await _context.Skills.ToListAsync();

            return View(skill);
        }
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create()

        {

            return View();
        }


        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSkillViewModel createskillViewModel)
        {


            if (!ModelState.IsValid)
                return View();


           Skill skill = new Skill
           {
Id = createskillViewModel.Id ,
Name = createskillViewModel.Name ,
            
           };
            await _context.Skills.AddAsync(skill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        public async Task<IActionResult> Delete(int Id)
        {
            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == Id);
            if (skill is null)
            {
                return NotFound();
            }
            DeleteSkillViewModel deleteskillViewModel = new DeleteSkillViewModel
            {
              Name= skill.Name ,
              Id= skill.Id,
              Percent=skill.Percent ,
              


            };
            return View(deleteskillViewModel);

        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var skill = await _context.Skills.FirstOrDefaultAsync(x => x.Id == id);
            if (skill is null)
                return NotFound();


        


            _context.Skills.Remove(skill);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int Id)
        {
            var skills = _context.Skills
                .Include(t => t.TeacherSkills)
                    .ThenInclude(ts => ts.Teacher)
                .FirstOrDefault(t => t.Id == Id);

            var detailSkillViewModel = _mapper.Map<DetailSkillViewModel>(skills);

            detailSkillViewModel.Teachers = skills.TeacherSkills
                .Select(ts => new DetailSkillViewModel.TeacherViewModel
                {
                    Name = ts.Teacher.Name
                })
                .ToList();

            return View(detailSkillViewModel);
        }

        //[Authorize(Roles = "Admin, Moderator")]
        //public async Task<IActionResult> Update(int id)
        //{
        //    var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == id);
        //    if (skill is null)
        //    {
        //        return NotFound();
        //    }
        //    UpdateSkillViewModel updateSkillViewModel = new UpdateSkillViewModel
        //    {
        //       Name=skill.Name,
        //        Percent= skill.Percent,
        //        Id=skill.Id
        //    };
        //    return View(updateSkillViewModel);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]





        //[Authorize(Roles = "Admin, Moderator")]

        //public async Task<IActionResult> Update(UpdateSkillViewModel updateSkillViewModel, int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == id);
        //    if (skill is null)
        //    {
        //        return NotFound();
        //    }


          
            

        //    skill.Name= updateSkillViewModel.Name;
        //    skill.Percent = updateSkillViewModel.Percent;



        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

    }
}
