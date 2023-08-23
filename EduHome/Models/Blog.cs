using EduHome.Models.common;

namespace EduHome.Models
{
    public class Blog :BaseEntity
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Time { get; set; }
        public string CommmentCount { get; set; }


    }
}
