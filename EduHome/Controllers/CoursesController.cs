using AutoMapper;
using EduHome.Contexts;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CoursesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
          
            var Courses =await _context.courses.ToListAsync();
            List<CourseViewModel> courseViewModels= _mapper.Map<List<CourseViewModel>>(Courses);

            return View(courseViewModels);
        }
        public async Task<IActionResult> Readmore(int Id)
        {
            var Courses = await _context.courses.Include(e => e.courseCategories).ThenInclude(e => e.Category).FirstOrDefaultAsync(e => e.Id == Id);
            if (Courses is null)
            {
                return NotFound();
            }
            var ReadmoreViewModel = _mapper.Map<ReadmoreViewModel>(Courses);


            return View(ReadmoreViewModel);
        }
    }
}
