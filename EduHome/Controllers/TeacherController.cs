using AutoMapper;
using EduHome.Contexts;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class TeacherController : Controller
    {
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

        public TeacherController (AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

		public async Task<IActionResult> Index()
        {
			var Teachers = await _context.Teachers.ToListAsync();
			List<TeacherViewModel> TeacherViewModel = _mapper.Map<List<TeacherViewModel>>(Teachers);
			return View(TeacherViewModel);
        }

        public IActionResult Details(int id)
        {
   
            var teacher = _context.Teachers.Include(t => t.SocialMedias).Include(t => t.TeacherSkills).ThenInclude(ts => ts.Skill).FirstOrDefault(t => t.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }

            var viewModel = new TeacherDetailViewModel
            {
                TeacherId = teacher.Id,
                Image = teacher.Image,
                Name = teacher.Name,
                Occupation= teacher.Occupation,
                Degree= teacher.Degree,
                Email= teacher.Email,
                Skype=teacher.skype,
                Faculty=teacher.Faculty,
                Phone=teacher.Phone,

                SocialMedias = teacher.SocialMedias,
                Skills = teacher.TeacherSkills.Select(ts => ts.Skill).ToList()
            };

            return View(viewModel);
        }

    }
}
