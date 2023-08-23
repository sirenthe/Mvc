using EduHome.Models.common;

namespace EduHome.Models
{
    public class Student :BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Occupation { get; set; }
        public string Description { get; set; }
        public string Degree { get; set; }
        public string Experinece { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public string skype { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public ICollection<StudentSocialMedia> StudentSocialMedias { get; set; }

        public ICollection<StudentSkill> StudentSkills { get; set; } = new List<StudentSkill>();
    }
}
