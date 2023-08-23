using EduHome.Models;

namespace EduHome.ViewModels
{
    public class DetailEventViewModel
    {
        public List<Speaker> Speakers { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public string Venue { get; set; }

    }
}
