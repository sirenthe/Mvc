using AutoMapper;
using EduHome.Areas.Admin.ViewModels.CourseViewModel;
using EduHome.Areas.Admin.ViewModels.EventViewModel;
using EduHome.Areas.Admin.ViewModels.SpeakerViewModel;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.Utils;
using EduHome.Utils.Enums;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers
{

	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CourseController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public CourseController(AppDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_mapper = mapper;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{

			var courses = await _context.courses.ToListAsync();
			return View(courses);
		}


		public async Task<IActionResult> Delete(int Id)
		{
			var courses = await _context.courses.FirstOrDefaultAsync(s => s.Id == Id);
			if (courses is null)
			{
				return NotFound();
			}

			var deleteCourseViewModel = _mapper.Map<DeleteCourseViewModel>(courses);

			return View(deleteCourseViewModel);
		}

		[HttpPost]
		[ActionName("Delete")]
		[HttpPost]

		public async Task<IActionResult> DeleteSpeaker(int id)
		{
			var course = await _context.courses.FirstOrDefaultAsync(x => x.Id == id);
			if (course is null)
				return NotFound();



			string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "course", course.Image);
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}

			_context.courses.Remove(course);

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Detail(int Id)
		{
			var course = await _context.courses.Include(e => e.courseCategories).ThenInclude(e => e.Category).FirstOrDefaultAsync(s => s.Id == Id);
			if (course is null)
			{
				return NotFound();
			}




			var viewModel = _mapper.Map<DetailCourseViewModel>(course);
			return View(viewModel);
		}



		[Authorize(Roles = "Admin, Moderator")]
		public async Task<IActionResult> Create()

		{
			List<ViewModels.CourseViewModel.CategoryViewModel> availableCategory = _context.Categories
		.Select(s => new ViewModels.CourseViewModel.CategoryViewModel
		{
			Id = s.Id,
			Name = s.Name
		})
		.ToList();

			var viewModel = new CreateCourseViewModel
			{
				AvailableCategory = availableCategory
			};

			return View(viewModel);


		}






		[Authorize(Roles = "Admin, Moderator")]
		public async Task<IActionResult> Update(int id)
		{
			//var course = await _context.courses.FirstOrDefaultAsync(s => s.Id == id);
			//if (course is null)
			//{
			//	return NotFound();
			//}
			Course course = await _context.courses
	.Include(c => c.courseCategories)
	.FirstOrDefaultAsync(c => c.Id == id);

			if (course == null) { return NotFound(); }
			List<ViewModels.CourseViewModel.UpdateCategoryViewModel> availableCategory = _context.Categories
		.Select(s => new ViewModels.CourseViewModel.UpdateCategoryViewModel
		{
			Id = s.Id,
			Name = s.Name
		})
		.ToList();

			var viewModel = new UpdateCourseViewModel
			{
				Name = course.Name,
			Description = course.Description,
			Fee= course.Fee,
			About=course.About,
			SkillLevel=course.SkillLevel,
			Students=course.Students,
			Assestments=course.Assestments,
			StartDate= course.StartDate,
			Duration= course.Duration,
			ClassDuration	= course.ClassDuration,
			Language=course.Language,
				AvailableCategory = availableCategory,
				SelectedCategoryIds=course.courseCategories.Select(n=>n.CategoryId.GetValueOrDefault()).ToList(),

			};

			return View(viewModel);

		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(UpdateCourseViewModel updateCourseViewModel, int id)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			//var course = await _context.courses.FirstOrDefaultAsync(b => b.Id == id);
			//if (course is null)
			//{
			//	return NotFound();
			//}

			Course course = await _context.courses.Include(e => e.courseCategories).FirstOrDefaultAsync(e => e.Id == updateCourseViewModel.Id);
			if(course == null) { return NotFound(); }

			if (updateCourseViewModel.Img is not null)
			{
				if (!updateCourseViewModel.Img.CheckFileType("image/"))
				{
					ModelState.AddModelError("Img", "sekil daxil edin");
					return View();
				}
				if (!updateCourseViewModel.Img.CheckFileSize(2))
				{
					ModelState.AddModelError("Img", "seklin olcusu uygun deyil")
						; return View();
				}
				string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "course", course.Image);
				if (System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);

					string fileName = $"{Guid.NewGuid()}-{updateCourseViewModel.Img.FileName}";
					string resultpath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "course", fileName);
					using (FileStream stream = new FileStream(resultpath, FileMode.Create))
					{
						await updateCourseViewModel.Img.CopyToAsync(stream);
					}
					course.Image = fileName;
				}
			}
			course.About = updateCourseViewModel.About;
			course.Duration= updateCourseViewModel.Duration;
			course.Assestments= updateCourseViewModel.Assestments;
			course.ClassDuration= updateCourseViewModel.ClassDuration;
			course.StartDate= updateCourseViewModel.StartDate;
			course.SkillLevel= updateCourseViewModel.SkillLevel;
			course.Language= updateCourseViewModel.Language;
			course.Description= updateCourseViewModel.Description;
			course.Students= updateCourseViewModel.Students;
			course.Fee= updateCourseViewModel.Fee;


			course.courseCategories = updateCourseViewModel.SelectedCategoryIds
				.Select(categoryId => new CourseCategory { CourseId = course.Id, CategoryId = categoryId })
				.ToList();


			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}











		[Authorize(Roles = "Admin, Moderator")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateCourseViewModel createCourseViewModel)
		{
			if (!ModelState.IsValid)
				return View();

			if (!createCourseViewModel.Img.CheckFileType("image/"))
			{
				ModelState.AddModelError("Image", "Please upload an image.");
				return View();
			}

			if (!createCourseViewModel.Img.CheckFileSize(2))
			{
				ModelState.AddModelError("Image", "Image size exceeds the limit.");
				return View();
			}

			string fileName = $"{Guid.NewGuid()}-{createCourseViewModel.Img.FileName}";
			string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "course", fileName);

			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				await createCourseViewModel.Img.CopyToAsync(fileStream);
			}

			Course course = _mapper.Map<CreateCourseViewModel, Course>(createCourseViewModel);
			course.Image = fileName;


			course.courseCategories = createCourseViewModel.SelectedCategoryIds
				.Select(categoryId => new CourseCategory { CourseId = course.Id, CategoryId = categoryId })
				.ToList();


			await _context.courses.AddAsync(course);
			await _context.SaveChangesAsync();






			return RedirectToAction(nameof(Index));

		}



	}
}