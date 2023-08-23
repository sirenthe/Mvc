using AutoMapper;
using EduHome.Areas.Admin.ViewModels.BlogViewModel;
using EduHome.Areas.Admin.ViewModels.SpeakerViewModel;
using EduHome.Areas.Admin.ViewModels.TeacherViewModel;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.Utils;
using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class SpeakerController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public SpeakerController(AppDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_mapper = mapper;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{

			var speakers = await _context.Speakers.ToListAsync();
			return View(speakers);
		}


        public async Task<IActionResult> Delete(int Id)
        {
            var speaker  = await _context.Speakers.FirstOrDefaultAsync(s => s.Id == Id);
            if (speaker is null)
            {
                return NotFound();
            }

            var deleteSpeakerViewModel = _mapper.Map<DeleteSpeakerViewModel>(speaker);

            return View(deleteSpeakerViewModel);
        }



        [HttpPost]
        [ActionName("Delete")]
        [HttpPost]

        public async Task<IActionResult> DeleteSpeaker(int id)
        {
            var speaker   = await _context.Speakers.FirstOrDefaultAsync(x => x.Id == id);
            if (speaker is null)
                return NotFound();



            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event", speaker.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _context.Speakers.Remove(speaker);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Detail(int Id)
        {
            var speakers = await _context.Speakers.Include(e => e.eventSpeakers).ThenInclude(e => e.Event).FirstOrDefaultAsync(s => s.Id == Id);
            if (speakers is null)
            {
                return NotFound();
            }




            var viewModel = _mapper.Map<DetailSpeakerViewModel>(speakers);
            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create()

        {

            return View();
        }

		[Authorize(Roles = "Admin, Moderator")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateSpeakerViewModel createspeakerViewModel)
		{


			if (!ModelState.IsValid)
				return View();
			if (!createspeakerViewModel.Image.CheckFileType("image/"))
			{
				ModelState.AddModelError("Img", "sekil daxil edin");
				return View();
			}
			if (!createspeakerViewModel.Image.CheckFileSize(2))
			{
				ModelState.AddModelError("Img", "seklin olcusu uygun deyil")
					; return View();
			}
			string fileName = $"{Guid.NewGuid()}-{createspeakerViewModel.Image.FileName}";

			string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event", fileName);

			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				await createspeakerViewModel.Image.CopyToAsync(fileStream);
			}







			Speaker speaker = new Speaker
			{
			Name= createspeakerViewModel.Name,
			Occupation=createspeakerViewModel.Occupation,
				Image = fileName,
			
			};
			await _context.Speakers.AddAsync(speaker);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}



		[Authorize(Roles = "Admin, Moderator")]
		public async Task<IActionResult> Update(int id)
		{
			var speaker = await _context.Speakers.FirstOrDefaultAsync(s => s.Id == id);
			if (speaker is null)
			{
				return NotFound();
			}
			UpdateSpeakerViewModel updatespeakerViewModel = new UpdateSpeakerViewModel
			{
			Id=speaker.Id,
		Name=speaker.Name,
		Occupation=speaker.Occupation,



			};
			return View(updatespeakerViewModel);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(UpdateSpeakerViewModel updatespeakerViewModel, int id)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var speaker = await _context.Speakers.FirstOrDefaultAsync(b => b.Id == id);
			if (speaker is null)
			{
				return NotFound();
			}

			if (updatespeakerViewModel.Image is not null)
			{
				if (!updatespeakerViewModel.Image.CheckFileType("image/"))
				{
					ModelState.AddModelError("Img", "sekil daxil edin");
					return View();
				}
				if (!updatespeakerViewModel.Image.CheckFileSize(2))
				{
					ModelState.AddModelError("Img", "seklin olcusu uygun deyil")
						; return View();
				}
				string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event", speaker.Image);
				if (System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);

					string fileName = $"{Guid.NewGuid()}-{updatespeakerViewModel.Image.FileName}";
					string resultpath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event", fileName);
					using (FileStream stream = new FileStream(resultpath, FileMode.Create))
					{
						await updatespeakerViewModel.Image.CopyToAsync(stream);
					}
					speaker.Image = fileName;
				}
			}
speaker.Name=updatespeakerViewModel.Name;
			speaker.Occupation=updatespeakerViewModel.Occupation;

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
	}
}
