using EduHome.Models;

namespace EduHome.Areas.Admin.ViewModels.SpeakerViewModel
{
    public class DetailSpeakerViewModel
    {
        public string? Name { get; set; }
        public string? Occupation { get; set; }
        public string? Image { get; set; }
     
        public List<Event> Events { get; set; }
    }
}
