using EduHome.Models.common;
using Org.BouncyCastle.Asn1.Mozilla;

namespace EduHome.Models
{
    public class Course :BaseSectionEntity
    {
        public string? Image { get; set; }
        public string ?Name { get; set; }
        public string ?Description { get; set; }
        public string ? About { get; set; }
        public string ?StartDate { get; set; }
        public string ?Duration { get; set; }
        public string ?ClassDuration { get; set; }
        public string? SkillLevel { get; set; }
        public string? Language { get; set; }
        public int ?Students { get; set; }
        public string ?Assestments { get; set; }
        public int? Fee { get; set; }
        public ICollection<CourseCategory> courseCategories{ get; set; }


    }
}
