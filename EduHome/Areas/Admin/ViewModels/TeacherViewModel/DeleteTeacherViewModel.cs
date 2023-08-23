using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.Admin.ViewModels.TeacherViewModel
{
    public class DeleteTeacherViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(300)]
        public string Occupation { get; set; }
        [Required]
        public string Img { get; set; }
    }
}
