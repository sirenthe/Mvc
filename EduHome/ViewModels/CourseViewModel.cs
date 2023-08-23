namespace EduHome.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; } 
        public string ?Image { get; set; }
        public string? Name{ get; set; }
        public string? Description { get; set; }
        public string ?About { get; set; }
        public string ?StartDate { get; set; }
        public string? Duration { get; set; }
        public string ?ClassDuration { get; set; }
        public string ?SkillLevel { get; set; }
        public string? Language { get; set; }
        public int ?Students { get; set; }
        public string ?Assestments { get; set; }
        public int ?Fee { get; set; }
    }
}
