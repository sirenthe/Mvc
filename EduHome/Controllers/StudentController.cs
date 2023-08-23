using AutoMapper;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;

namespace EduHome.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public StudentController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();
            List<StudentViewModel> studentViewModel = _mapper.Map<List<StudentViewModel>>(students);
            return View(studentViewModel);
        }


        public IActionResult Details(int id)
        {

            var student = _context.Students.Include(t => t.StudentSocialMedias).Include(t => t.StudentSkills).ThenInclude(ts => ts.Skill2).FirstOrDefault(t => t.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var viewModel = new StudentDetailViewModel
            {
                StudentId = student.Id,
                Image = student.Image,
                Name = student.Name,
                Occupation = student.Occupation,
                Degree = student.Degree,
                Email = student.Email,
                Skype = student.skype,
                Faculty = student.Faculty,
                Phone = student.Phone,

                StudentSocialMedias = student.StudentSocialMedias,
                Skills2 = student.StudentSkills.Select(ts => ts.Skill2).ToList()
            };

            return View(viewModel);
        }
    }
}
