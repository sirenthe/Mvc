using EduHome.Models;

namespace EduHome.Areas.Admin.ViewModels.EventViewModel
{
	public class UpdateEventViewModel
	{
	public int Id { get; set; }	
		public string? Name { get; set; }
		public string? Description { get; set; }
		public IFormFile? Image { get; set; }
		public string? Time { get; set; }
		public string? Date { get; set; }
		public string? Venue { get; set; }
		public List<int> SelectedSpeakerIds { get; set; }

		public List<UpdateSpeakerViewModel>? AvailableSpeakers { get; set; }

		
	}
	public class UpdateSpeakerViewModel
	{
		public int ?Id { get; set; }
		public string ?Name { get; set; }
	}
}
