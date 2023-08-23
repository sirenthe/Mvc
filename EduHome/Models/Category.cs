using EduHome.Models.common;

namespace EduHome.Models
{
    public class Category :BaseEntity
    {
        public string? Name { get; set; }
        public ICollection<CourseCategory> courseCategories { get; set; }
    }
}
