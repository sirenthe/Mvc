using EduHome.Contexts;
using Microsoft.AspNetCore.Mvc;
using EduHome.Models;
using Microsoft.EntityFrameworkCore;
using EduHome.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using EduHome.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using EduHome.Identity;

namespace EduHome.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly UserManager<AppUser> _userManager;

        public EventController (AppDbContext context, IMapper mapper, IMailService mailService, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _mailService = mailService;
            _userManager = userManager;
         
        }
      
        public async Task<IActionResult> Index()
        {
            var allEvents = await _context.Events.ToListAsync();

     
            var sortedEvents = allEvents.OrderByDescending(e => e.CreatedDate).ToList();

            int numberOfEventsToShow = 9; 
            var eventsToShow = sortedEvents.Take(numberOfEventsToShow).ToList();

            List<EventViewModel> EventsViewModel = _mapper.Map<List<EventViewModel>>(eventsToShow);

            return View(EventsViewModel);
        }

        public async Task<IActionResult> JoinNow(int Id)
        {
            var Event = await _context.Events.Include(e=>e.eventSpeakers).ThenInclude(e=>e.Speaker).FirstOrDefaultAsync(e=>e.Id==Id);
            if(Event is null)
            {
                return NotFound();
            }
          var DetailEventViewModel =_mapper.Map<DetailEventViewModel?>(Event);


            return View(DetailEventViewModel);
        }




		public async Task<IActionResult> Subscribe(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return BadRequest("cannot be null");
			}

			if (!User.Identity.IsAuthenticated)
			{

				return RedirectToAction("Login", "Auth");
			}

			var existingSubscriber = await _context.Subscriber.FirstOrDefaultAsync(s => s.Email == email);

			if (existingSubscriber != null)
			{
				if (existingSubscriber.IsSubscribed)
				{
					return BadRequest("subscribedir ");
				}
				else
				{
					existingSubscriber.IsSubscribed = true;
				}
			}
			else
			{
				var newSubscriber = new Subscriber
				{
					Email = email,
					IsSubscribed = true
				};

				await _context.Subscriber.AddAsync(newSubscriber);
			}

			await _context.SaveChangesAsync();

			return RedirectToAction("Index", "Home");
		}
	







	}
}
