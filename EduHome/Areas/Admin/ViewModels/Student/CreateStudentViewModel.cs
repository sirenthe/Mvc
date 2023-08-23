using EduHome.Models;

namespace EduHome.Areas.Admin.ViewModels.Student
{
    public class CreateStudentViewModel
    {
        public int? Id { get; set; }
        public IFormFile? Image { get; set; }
        public string? Name { get; set; }
        public string? Occupation { get; set; }
        public string? Description { get; set; }
        public string? Degree { get; set; }
        public string? Experinece { get; set; }
        public string? Hobbies { get; set; }
        public string? Faculty { get; set; }
        public string? skype { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public List<int>? SelectedSocialMedia { get; set; }

		public List<int> SelectedSkills { get; set; } = new List<int>();
		public List<int> SkillPercentages { get; set; } = new List<int>();
		public List<StudentSocialMedia> ?SocialMediaOptions { get; set; }
		public List<Skill2> SkillOptions { get; set; } = new List<Skill2>();

	}
}
