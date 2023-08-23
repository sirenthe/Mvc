using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.Admin.ViewModels.CourseViewModel
{
	public class DeleteCourseViewModel
	{
		[Required]
		[MaxLength(100)]
		public string Name { get; set; }
		[Required]
		[MaxLength(300)]
		public string Description { get; set; }
		[Required]
		public string Img { get; set; }
	}
}
