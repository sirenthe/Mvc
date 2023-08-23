using AutoMapper;
using EduHome.Contexts;
using EduHome.Identity;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
    
       
        public BlogController(AppDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }
      
        public async Task<IActionResult> Index()
        {
            var allBlogs = await _context.Blogs.ToListAsync();

            int numberOfBlogsToShow = 9;
            var blogsToShow = allBlogs.Take(numberOfBlogsToShow).ToList();

            List<BlogViewModel> BlogsViewModel = _mapper.Map<List<BlogViewModel>>(blogsToShow);

            return View(BlogsViewModel);
        }
        public async Task<IActionResult> JoinNow(int Id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(e => e.Id == Id);
            if (blog is null)
            {
                return NotFound();
            }
            var DetailBlogViewModel = _mapper.Map<DetailBlogViewModel?>(blog);


            return View(DetailBlogViewModel);
        }

    }
}
