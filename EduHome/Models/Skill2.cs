using EduHome.Models.common;

namespace EduHome.Models
{
    public class Skill2 :BaseEntity
    {
             public string Name { get; set; }

        public ICollection<StudentSkill> StudentSkills { get; set; } = new List<StudentSkill>();
    }
}
