using EduHome.Models;

namespace EduHome.Areas.Admin.ViewModels.EventViewModel
{
    public class DetailEventViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Time { get; set; }
        public string? Date { get; set; }
        public string? Venue { get; set; }
        public List<Speaker> Speakers { get; set; }
    }
}
