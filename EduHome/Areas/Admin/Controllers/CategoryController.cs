using AutoMapper;
using EduHome.Areas.Admin.ViewModels.CategoryViewModel;
using EduHome.Areas.Admin.ViewModels.SkillViewModel;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CategoryController : Controller
	{

	

		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public CategoryController(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;

		}
		public async Task<IActionResult> Index()
		{

			var category = await _context.Categories.ToListAsync();

			return View(category);
		}

        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create()

        {

            return View();
        }


        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel createcategoryViewModel)
        {


            if (!ModelState.IsValid)
                return View();


            Category category = new Category
            {
Name = createcategoryViewModel.Name };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		public async Task<IActionResult> Delete(int Id)
		{
			var category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == Id);
			if (category is null)
			{
				return NotFound();
			}
			DeleteCategoryViewModel deletecategoryViewModel = new DeleteCategoryViewModel
			{
				Name = category.Name,
				Id = category.Id,
			



			};
			return View(deletecategoryViewModel);

		}

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category= await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
                return NotFound();





            var relatedCourseCategories = _context.courseCategories.Where(cc => cc.CategoryId == id);
            _context.courseCategories.RemoveRange(relatedCourseCategories);
            _context.Categories.Remove(category);



            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Detail(int Id)
        {
            var category = _context.Categories
                .Include(t => t.courseCategories)
                    .ThenInclude(ts => ts.Course)
                .FirstOrDefault(t => t.Id == Id);

            var detailCategoryViewModel = _mapper.Map<DetailCategoryViewModel>(category);

            detailCategoryViewModel.Courses = category.courseCategories
                .Select(ts => new DetailCategoryViewModel.CourseViewModel
                {
                    Name = ts.Course.Name
                })
                .ToList();

            return View(detailCategoryViewModel);
        }
    }
}
