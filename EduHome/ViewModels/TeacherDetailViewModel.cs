using EduHome.Models;

namespace EduHome.ViewModels
{
    public class TeacherDetailViewModel
    {
        public int TeacherId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Occupation { get; set; }
        public string Description { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public string Skype { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
 
        public ICollection<SocialMedia> SocialMedias { get; set; }

    
        public ICollection<Skill> Skills { get; set; }
    }
}
