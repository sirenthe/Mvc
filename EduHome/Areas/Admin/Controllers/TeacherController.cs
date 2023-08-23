using AutoMapper;
using EduHome.Areas.Admin.ViewModels.BlogViewModel;
using EduHome.Areas.Admin.ViewModels.CourseViewModel;
using EduHome.Areas.Admin.ViewModels.EventViewModel;
using EduHome.Areas.Admin.ViewModels.TeacherViewModel;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.Utils;
using EduHome.Utils.Enums;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static EduHome.ViewModels.DetailTeacherViewModel;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TeacherController : Controller
	{
      
        private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHostEnvironment;

	public TeacherController(AppDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment )
		{
			_context= context;
			_mapper= mapper;
			_webHostEnvironment= webHostEnvironment;
		}
		public async Task<IActionResult> Index()
		{

			var teachers = await _context.Teachers.ToListAsync();
			return View(teachers);
		}


        public async Task<IActionResult> Delete(int Id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(s => s.Id == Id);
            if (teacher is null)
            {
                return NotFound();
            }

            var deleteTeacherViewModel = _mapper.Map<DeleteTeacherViewModel>(teacher);

            return View(deleteTeacherViewModel);
        }



        [HttpPost]
        [ActionName("Delete")]
        [HttpPost]

        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (teacher is null)
                return NotFound();



            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "teacher", teacher.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _context.Teachers.Remove(teacher);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		public async Task<IActionResult> Detail(int Id)
		{
			var teacher = _context.Teachers
				.Include(t => t.SocialMedias)
				.Include(t => t.TeacherSkills)
					.ThenInclude(ts => ts.Skill)
				.FirstOrDefault(t => t.Id == Id);

			var detailTeacherViewModel = _mapper.Map<DetailTeacherViewModel>(teacher);


			detailTeacherViewModel.SocialMedias = teacher.SocialMedias
				.Select(sm => new SocialMediaViewModel
				{
					Name = sm.Name,
					Url = sm.Url
				})
				.ToList();

			detailTeacherViewModel.Skills = teacher.TeacherSkills
				.Select(ts => new SkillViewModel
				{
					Name = ts.Skill.Name
				})
				.ToList();

			return View(detailTeacherViewModel);
		}






		//	[Authorize(Roles = "Admin, Moderator")]
		//	public async Task<IActionResult> Create()
		//	{
		//		List<ViewModels.TeacherViewModel.TeacherSkillViewModel> availableSkills = _context.Skills
		//			.Select(s => new ViewModels.TeacherViewModel.TeacherSkillViewModel
		//			{
		//				Id = s.Id,
		//				Name = s.Name
		//			})
		//			.ToList();



		//		var viewModel = new CreateTeacherViewModel
		//		{
		//			AvailableSkill = availableSkills,

		//		};

		//		return View(viewModel);
		//	}





		//[Authorize(Roles = "Admin, Moderator")]
		//	[HttpPost]
		//	[ValidateAntiForgeryToken]
		//	public async Task<IActionResult> Create(CreateTeacherViewModel createTeacherViewModel)
		//	{
		//		if (!ModelState.IsValid)
		//			return View();

		//		if (!createTeacherViewModel.Image.CheckFileType("image/"))
		//		{
		//			ModelState.AddModelError("Image", "Please upload an image.");
		//			return View();
		//		}

		//		if (!createTeacherViewModel.Image.CheckFileSize(2))
		//		{
		//			ModelState.AddModelError("Image", "Image size exceeds the limit.");
		//			return View();
		//		}

		//		string fileName = $"{Guid.NewGuid()}-{createTeacherViewModel.Image.FileName}";
		//		string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "teacher", fileName);

		//		using (FileStream fileStream = new FileStream(path, FileMode.Create))
		//		{
		//			await createTeacherViewModel.Image.CopyToAsync(fileStream);
		//		}

		//		Teacher teacher = _mapper.Map<CreateTeacherViewModel, Teacher>(createTeacherViewModel);

		//		teacher.Image = fileName;


		//		await _context.Teachers.AddAsync(teacher);
		//		await _context.SaveChangesAsync();

		//		return RedirectToAction(nameof(Index));


		//	}


		[Authorize(Roles = "Admin, Moderator")]
		public async Task<IActionResult> Create()
		{
			List<ViewModels.TeacherViewModel.TeacherSkillViewModel> availableSkills = _context.Skills
				.Select(s => new ViewModels.TeacherViewModel.TeacherSkillViewModel
				{
					Id = s.Id,
					Name = s.Name
				})
				.ToList();

			List<ViewModels.TeacherViewModel.TeacherSocialMediaViewModel> availableSocialMedia = _context.SocialMedias
				.Select(sm => new ViewModels.TeacherViewModel.TeacherSocialMediaViewModel
				{
					Id = sm.Id,
					Name = sm.Name
				})
				.ToList();

			var viewModel = new CreateTeacherViewModel
			{
				AvailableSkill = availableSkills,
				AvailableSocialMedia = availableSocialMedia 
			};

			return View(viewModel);
		}

		[Authorize(Roles = "Admin, Moderator")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateTeacherViewModel createTeacherViewModel)
		{
			if (!ModelState.IsValid)
				return View(createTeacherViewModel);

			if (!createTeacherViewModel.Image.CheckFileType("image/"))
			{
				ModelState.AddModelError("Image", "Please upload an image.");
				return View(createTeacherViewModel);
			}

			if (!createTeacherViewModel.Image.CheckFileSize(2))
			{
				ModelState.AddModelError("Image", "Image size exceeds the limit.");
				return View(createTeacherViewModel);
			}

			string fileName = $"{Guid.NewGuid()}-{createTeacherViewModel.Image.FileName}";
			string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "teacher", fileName);

			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				await createTeacherViewModel.Image.CopyToAsync(fileStream);
			}

			Teacher teacher = _mapper.Map<CreateTeacherViewModel, Teacher>(createTeacherViewModel);
			teacher.Image = fileName;

			if (createTeacherViewModel.SelectedSocialMediaIds != null)
			{
				teacher.SocialMedias = _context.SocialMedias
					.Where(sm => createTeacherViewModel.SelectedSocialMediaIds.Contains(sm.Id))
					.ToList();
			}

			await _context.Teachers.AddAsync(teacher);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		



		[Authorize(Roles = "Admin, Moderator")]
		public async Task<IActionResult> Update(int id)
		{
			var teacher  = await _context.Teachers.FirstOrDefaultAsync(s => s.Id == id);
			if (teacher  is null)
			{
				return NotFound();
			}
			UpdateViewModel updateViewModel = new UpdateViewModel
			{
				 Id = teacher.Id,
				Name = teacher.Name,
				Occupation = teacher.Occupation,
				Description = teacher.Description,
				Degree = teacher.Degree,
				Experinece = teacher.Experinece,
				Hobbies = teacher.Hobbies,
				Faculty = teacher.Faculty,
				skype = teacher.skype,
				Phone = teacher.Phone,
				Email = teacher.Email,


			};
			return View(updateViewModel);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(UpdateViewModel updateViewModel, int id)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var teacher  = await _context.Teachers.FirstOrDefaultAsync(b => b.Id == id);
			if (teacher is null)
			{
				return NotFound();
			}

			if (updateViewModel.Image is not null)
			{
				if (!updateViewModel.Image.CheckFileType("image/"))
				{
					ModelState.AddModelError("Img", "sekil daxil edin");
					return View();
				}
				if (!updateViewModel.Image.CheckFileSize(2))
				{
					ModelState.AddModelError("Img", "seklin olcusu uygun deyil")
						; return View();
				}
				string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "teacher", teacher.Image);
				if (System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);

					string fileName = $"{Guid.NewGuid()}-{updateViewModel.Image.FileName}";
					string resultpath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "teacher", fileName);
					using (FileStream stream = new FileStream(resultpath, FileMode.Create))
					{
						await updateViewModel.Image.CopyToAsync(stream);
					}
					teacher.Image = fileName;
				}
			}

			teacher.Occupation = updateViewModel.Occupation;
			teacher.Faculty = updateViewModel.Faculty;
			teacher.Hobbies = updateViewModel.Hobbies;
			teacher.Description = updateViewModel.Description;
			teacher.Name = updateViewModel.Name;
			teacher.Degree = updateViewModel.Degree;
			teacher.Experinece = updateViewModel.Experinece;
			teacher.skype = updateViewModel.skype;
			teacher.Email = updateViewModel.Email; teacher.Phone = updateViewModel.Phone;
			teacher.Id = (int)updateViewModel.Id;

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

	}
}
