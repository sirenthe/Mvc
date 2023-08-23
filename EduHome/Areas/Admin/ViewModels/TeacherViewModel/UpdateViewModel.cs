namespace EduHome.Areas.Admin.ViewModels.TeacherViewModel
{
	public class UpdateViewModel
	{
		public int? Id { get; set; }
		public IFormFile ?Image { get; set; }
		public string Name { get; set; }
		public string? Occupation { get; set; }
		public string? Description { get; set; }
		public string? Degree { get; set; }
		public string? Experinece { get; set; }
		public string? Hobbies { get; set; }
		public string? Faculty { get; set; }
		public string? skype { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }

		public List<int>? SelectedSkillIds { get; set; }
		public List<int> SkillPercentages { get; set; }

		public List<UpdateTeacherSkillViewModel>? AvailableSkill { get; set; }

		public List<int>? SelectedSocialMediaIds { get; set; }

		public List<UpdateTeacherSocialMediaViewModel>? AvailableSocialMedia { get; set; }

	}
	public class UpdateTeacherSkillViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

	}
	public class UpdateTeacherSocialMediaViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

	}
}

