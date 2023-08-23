using EduHome.Models.common;

namespace EduHome.Models
{
    public class StudentSkill :BaseEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int Skill2Id { get; set; }
        public Skill2 Skill2 { get; set; }

        public double Percent { get; set; }
    }
}
