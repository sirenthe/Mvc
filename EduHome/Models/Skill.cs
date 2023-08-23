using EduHome.Models.common;

namespace EduHome.Models
{
	public class Skill :BaseEntity
	{
		public string Name { get; set; }
		public double Percent { get; set; }

		public ICollection<TeacherSkill> TeacherSkills { get; set; }
	}
}
