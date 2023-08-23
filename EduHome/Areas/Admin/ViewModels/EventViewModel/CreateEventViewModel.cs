namespace EduHome.Areas.Admin.ViewModels.EventViewModel
{
	public class CreateEventViewModel
	{
		public string? Name { get; set; }
		public string? Description { get; set; }
		public IFormFile? Image { get; set; }
		public string? Time { get; set; }
		public string? Date { get; set; }
		public string? Venue { get; set; }
		public List<int> ?SelectedSpeakerIds { get; set; }

		public List<SpeakerViewModel>? AvailableSpeakers { get; set; }
	}
	public class SpeakerViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
