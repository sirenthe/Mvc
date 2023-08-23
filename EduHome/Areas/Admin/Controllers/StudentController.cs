using AutoMapper;
using EduHome.Areas.Admin.ViewModels.Student;
using EduHome.Areas.Admin.ViewModels.TeacherViewModel;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.Utils;
using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StudentController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public StudentController(AppDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task< IActionResult> Index()
        {

            var student= await _context.Students.ToListAsync();
            return View(student);
           
        }




		public IActionResult Create()
		{
			var socialMediaOptions = _context.studentSocialMedia.ToList();
			var skillOptions = _context.skill2s.ToList();

			var viewModel = new CreateStudentViewModel
			{
				SocialMediaOptions = socialMediaOptions,
				SkillOptions = skillOptions,
	
			};

			return View(viewModel);
		}


		[HttpPost]
		//      public async Task<IActionResult> CreateAsync(CreateStudentViewModel viewModel)
		//      {
		//          if(!ModelState.IsValid)
		//          {
		//              return View();
		//          }
		//	//if (!viewModel.Image.checkfiletype("image/"))
		//	//{
		//	//	modelstate.addmodelerror("image", "please upload an image.");
		//	//	return view();
		//	//}

		//	//if (!viewmodel.image.checkfilesize(2))
		//	//{
		//	//	modelstate.addmodelerror("image", "image size exceeds the limit.");
		//	//	return view();
		//	//}
		//	string fileName = $"{Guid.NewGuid()}-{viewModel.Image.FileName}";
		//	string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "teacher", fileName);

		//	using (FileStream fileStream = new FileStream(path, FileMode.Create))
		//	{
		//		await viewModel.Image.CopyToAsync(fileStream);
		//	}


		//	var newStudent = _mapper.Map<Student>(viewModel);
		//	newStudent.Image = fileName;













		//	_context.SaveChanges();


		//	//for (int i = 0; i < viewModel.SelectedSkills.Count; i++)
		// //             {
		// //                 var skillId = viewModel.SelectedSkills[i];
		// //                 var skillPercentage = viewModel.SkillPercentages[i];


		// //             }

		// //             _context.Students.Add(newStudent);
		// //             _context.SaveChanges();

		//	for (int i = 0; i < viewModel.SelectedSkills.Count; i++)
		//	{
		//		var skillId = viewModel.SelectedSkills[i];
		//		var skillPercentage = viewModel.SkillPercentages[i];

		//		var studentSkill = new StudentSkill
		//		{
		//			StudentId = newStudent.Id,
		//			Skill2Id = skillId,
		//			Percent = skillPercentage
		//		};

		//		_context.studentSkills.Add(studentSkill);
		//	}
		//	_context.SaveChanges();


		//	return RedirectToAction("Index", "Student");
		//}

		[HttpPost]
		public async Task<IActionResult> CreateAsync(CreateStudentViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			if (viewModel.Image == null)
			{
				ModelState.AddModelError("Image", "Please upload an image.");
				return View();
			}

			if (!viewModel.Image.CheckFileType("image/"))
			{
				ModelState.AddModelError("Image", "Invalid image format. Please upload an image.");
				return View();
			}

			if (!viewModel.Image.CheckFileSize(2))
			{
				ModelState.AddModelError("Image", "Image size exceeds the limit.");
				return View();
			}

			string fileName = $"{Guid.NewGuid()}-{viewModel.Image.FileName}";
			string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "teacher", fileName);

			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				await viewModel.Image.CopyToAsync(fileStream);
			}

			var newStudent = _mapper.Map<Student>(viewModel);
			newStudent.Image = fileName;

			_context.SaveChanges();


			return RedirectToAction("Index", "Student");
		}


	}
}
