using System.Xml.Linq;
using AutoMapper;
using EduHome.Areas.Admin.ViewModels.CourseViewModel;
using EduHome.Areas.Admin.ViewModels.EventViewModel;
using EduHome.Areas.Admin.ViewModels.SpeakerViewModel;
using EduHome.Areas.Admin.ViewModels.TeacherViewModel;
using EduHome.Contexts;
using EduHome.Controllers;
using EduHome.Models;
using EduHome.Services.Interfaces;
using EduHome.Utils;
using EduHome.Utils.Enums;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduHome.Areas.Admin.Controllers
{

	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class EventController : Controller
	{

		private readonly AppDbContext _context;
		private readonly IMailService _mailService;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public EventController(AppDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, IMailService mailService)
		{
			_context = context;
			_mapper = mapper;
			_webHostEnvironment = webHostEnvironment;
			_mailService = mailService;
		}
		public async Task<IActionResult> Index()
		{

			var events = await _context.Events.ToListAsync();
			return View(events);
		}

		public async Task<IActionResult> Delete(int Id)
		{
			var events = await _context.Events.FirstOrDefaultAsync(s => s.Id == Id);
			if (events is null)
			{
				return NotFound();
			}

			var deleteEventViewModel = _mapper.Map<DeleteEventViewModel>(events);

			return View(deleteEventViewModel);
		}


		[HttpPost]
		[ActionName("Delete")]
		[HttpPost]

		public async Task<IActionResult> DeleteEvent(int id)
		{
			var events = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
			if (events is null)
				return NotFound();



			string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event", events.Image);
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}

			_context.Events.Remove(events);

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}



		public async Task<IActionResult> Detail(int Id)
		{
			var events = await _context.Events.Include(e => e.eventSpeakers).ThenInclude(e => e.Speaker).FirstOrDefaultAsync(s => s.Id == Id);
			if (events is null)
			{
				return NotFound();
			}




			var viewModel = _mapper.Map<ViewModels.EventViewModel.DetailEventViewModel>(events);
			return View(viewModel);
		}




		[Authorize(Roles = "Admin, Moderator")]
		public async Task<IActionResult> Create()

		{
			List<ViewModels.EventViewModel.SpeakerViewModel> availableSpeakers = _context.Speakers
		.Select(s => new ViewModels.EventViewModel.SpeakerViewModel
		{
			Id = s.Id,
			Name = s.Name
		})
		.ToList();

			var viewModel = new CreateEventViewModel
			{
				AvailableSpeakers = availableSpeakers
			};

			return View(viewModel);


		}






		[Authorize(Roles = "Admin, Moderator")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateEventViewModel createEventViewModel)
		{
			if (!ModelState.IsValid)
				return View();

			if (!createEventViewModel.Image.CheckFileType("image/"))
			{
				ModelState.AddModelError("Image", "Please upload an image.");
				return View();
			}

			if (!createEventViewModel.Image.CheckFileSize(2))
			{
				ModelState.AddModelError("Image", "Image size exceeds the limit.");
				return View();
			}

			string fileName = $"{Guid.NewGuid()}-{createEventViewModel.Image.FileName}";
			string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event", fileName);

			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				await createEventViewModel.Image.CopyToAsync(fileStream);
			}

			Event events = _mapper.Map<CreateEventViewModel, Event>(createEventViewModel);
			events.Image = fileName;


			events.eventSpeakers = createEventViewModel.SelectedSpeakerIds
				.Select(speakerId => new EventSpeaker { EventId = events.Id, SpeakerId = speakerId })
				.ToList();

			await _context.Events.AddAsync(events);
			await _context.SaveChangesAsync();
			var subscribedUsers = await _context.Subscriber.Where(s => s.IsSubscribed).ToListAsync();


			foreach (var user in subscribedUsers)
			{


				var eventDetailsLink = $"https://localhost:7239/Event";

				var mailRequest = new MailRequest
				{
					ToEmail = user.Email,
					Subject = $"New Event: {events.Name}",
					Body = $"Check out the new event '{events.Name}' <a href='{eventDetailsLink}'>Event Details</a>"
				};

				await _mailService.SendEmailAsync(mailRequest);


				
			}
			return RedirectToAction(nameof(Index));

		}






		//	[Authorize(Roles = "Admin, Moderator")]
		//	public async Task<IActionResult> Update(int id)
		//	{
		//		var events = await _context.Events.FirstOrDefaultAsync(s => s.Id == id);
		//		if (events is null)
		//		{
		//			return NotFound();
		//		}
		//		List<ViewModels.EventViewModel.UpdateSpeakerViewModel> availableSpeakers = _context.Speakers
		//.Select(s => new ViewModels.EventViewModel.UpdateSpeakerViewModel
		//{
		//	Id = s.Id,
		//	Name = s.Name
		//})
		//.ToList();
		//		UpdateEventViewModel updateEventViewModel = new UpdateEventViewModel
		//		{
		//			Name = events.Name,
		//			Venue = events.Venue,
		//			Time = events.Time,
		//			Description = events.Description,
		//			Date = events.Date,
		//			AvailableSpeakers = availableSpeakers


		//		};






		//		return View(updateEventViewModel);
		//	}

		//	[Authorize(Roles = "Admin, Moderator")]


		//	[HttpPost]
		//	[ValidateAntiForgeryToken]
		//	public async Task<IActionResult> Update(UpdateEventViewModel updateEventViewModel, int id)
		//	{
		//		if (!ModelState.IsValid)
		//		{
		//			return View();
		//		}

		//		var events = await _context.Events.FirstOrDefaultAsync(b => b.Id == id);
		//		if (events is null)
		//		{
		//			return NotFound();
		//		}

		//		if (updateEventViewModel.Image is not null)
		//		{
		//			if (!updateEventViewModel.Image.CheckFileType("image/"))
		//			{
		//				ModelState.AddModelError("Img", "sekil daxil edin");
		//				return View();
		//			}
		//			if (!updateEventViewModel.Image.CheckFileSize(2))
		//			{
		//				ModelState.AddModelError("Img", "seklin olcusu uygun deyil")
		//					; return View();
		//			}
		//			string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event", events.Image);
		//			if (System.IO.File.Exists(path))
		//			{
		//				System.IO.File.Delete(path);

		//				string fileName = $"{Guid.NewGuid()}-{updateEventViewModel.Image.FileName}";
		//				string resultpath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event", fileName);
		//				using (FileStream stream = new FileStream(resultpath, FileMode.Create))
		//				{
		//					await updateEventViewModel.Image.CopyToAsync(stream);
		//				}
		//				events.Image = fileName;
		//			}
		//		}
		//		events.Name = updateEventViewModel.Name;
		//		events.Description = updateEventViewModel.Description;
		//		events.Date = updateEventViewModel.Date;
		//		events.Venue= updateEventViewModel.Venue;
		//		events.Time= updateEventViewModel.Time;


		//		events.eventSpeakers = updateEventViewModel.SelectedSpeakerIds
		//			.Select(speakerId => new EventSpeaker { EventId = events.Id, SpeakerId = speakerId })
		//			.ToList();

		//		await _context.SaveChangesAsync();

		//		return RedirectToAction(nameof(Index));
		//	}



		[Authorize(Roles = "Admin, Moderator")]
		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			Event existingEvent = await _context.Events
				.Include(e => e.eventSpeakers)
				.FirstOrDefaultAsync(e => e.Id == id);

			if (existingEvent == null)
			{
			
				return NotFound();
			}

			List<ViewModels.EventViewModel.UpdateSpeakerViewModel> availableSpeakers = _context.Speakers
				.Select(s => new ViewModels.EventViewModel.UpdateSpeakerViewModel
				{
					Id = s.Id,
					Name = s.Name
				})
				.ToList();

			var viewModel = new UpdateEventViewModel
			{
				Name = existingEvent.Name,
				Description= existingEvent.Description,
				Venue = existingEvent.Venue,
							Time = existingEvent.Time,
						Date = existingEvent.Date,
				AvailableSpeakers = availableSpeakers,
				SelectedSpeakerIds = existingEvent.eventSpeakers
			.Select(es => es.SpeakerId.GetValueOrDefault()) 
			.ToList()
			};


			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "Admin, Moderator")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(UpdateEventViewModel updateEventViewModel)
		{
			if (!ModelState.IsValid)
				return View(updateEventViewModel);

			Event events = await _context.Events
				.Include(e => e.eventSpeakers)
				.FirstOrDefaultAsync(e => e.Id == updateEventViewModel.Id);

			if (events == null)
			{
				
				return NotFound();
			}



			if (updateEventViewModel.Image is not null)
			{
				if (!updateEventViewModel.Image.CheckFileType("image/"))
				{
					ModelState.AddModelError("Img", "sekil daxil edin");
					return View();
				}
				if (!updateEventViewModel.Image.CheckFileSize(2))
				{
					ModelState.AddModelError("Img", "seklin olcusu uygun deyil")
						; return View();
				}
				string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event", events.Image);
				if (System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);

					string fileName = $"{Guid.NewGuid()}-{updateEventViewModel.Image.FileName}";
					string resultpath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event", fileName);
					using (FileStream stream = new FileStream(resultpath, FileMode.Create))
					{
						await updateEventViewModel.Image.CopyToAsync(stream);
					}
					events.Image = fileName;
				}
			}
			events.Name = updateEventViewModel.Name;
			events.Description = updateEventViewModel.Description;
			events.Date = updateEventViewModel.Date;
			events.Venue = updateEventViewModel.Venue;
			events.Time = updateEventViewModel.Time;


			events.eventSpeakers = updateEventViewModel.SelectedSpeakerIds
				.Select(speakerId => new EventSpeaker { EventId = events.Id, SpeakerId = speakerId })
				.ToList();

			_context.Events.Update(events);
			await _context.SaveChangesAsync();



			return RedirectToAction(nameof(Index));
		}

	}
}


