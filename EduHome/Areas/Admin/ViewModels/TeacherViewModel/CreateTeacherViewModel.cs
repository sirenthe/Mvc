namespace EduHome.Areas.Admin.ViewModels.TeacherViewModel
{
	public class CreateTeacherViewModel
	{
		public int? Id { get; set; }
		public IFormFile ?Image { get; set; }
		public string ?Name { get; set; }
		public string? Occupation { get; set; }
		public string ?Description { get; set; }
		public string ?Degree { get; set; }
		public string ?Experinece { get; set; }
		public string ?Hobbies { get; set; }
		public string ?Faculty { get; set; }
		public string ?skype { get; set; }
		public string ?Phone { get; set; }
		public string ?Email { get; set; }

		public List<int>? SelectedSkillIds { get; set; }
		public List<int> SkillPercentages { get; set; }

		public List<TeacherSkillViewModel>? AvailableSkill { get; set; }

		public List<int>? SelectedSocialMediaIds { get; set; }
		
		public List<TeacherSocialMediaViewModel>? AvailableSocialMedia { get; set; }

	}
	public class TeacherSkillViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

	}
	public class TeacherSocialMediaViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		
	}
}
