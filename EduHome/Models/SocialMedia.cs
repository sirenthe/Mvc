using EduHome.Models.common;

namespace EduHome.Models
{
	public class SocialMedia :BaseEntity
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public int TeacherId { get; set; }
		public Teacher Teacher { get; set; }
	}
}
