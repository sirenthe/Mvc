using EduHome.Models.common;

namespace EduHome.Models
{
    public class StudentSocialMedia :BaseEntity

    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
