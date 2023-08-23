using System.ComponentModel.DataAnnotations;
using EduHome.Models;

namespace EduHome.Areas.Admin.ViewModels.CourseViewModel
{
	public class CreateCourseViewModel
	{
		public List<Category> ?Categories { get; set; }

		public string Name { get; set; }


		public string Description { get; set; }

	
		public IFormFile Img { get; set; }

		public string About { get; set; }
		public string? StartDate { get; set; }
		public string? Duration { get; set; }
		public string? ClassDuration { get; set; }
		public string? SkillLevel { get; set; }
		public string? Language { get; set; }
		public int? Students { get; set; }
		public string? Assestments { get; set; }
		public int? Fee { get; set; }
		public List<int>? SelectedCategoryIds { get; set; }

		public List<CategoryViewModel>? AvailableCategory { get; set; }

	}
	public class CategoryViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
