using AutoMapper;
using EduHome.Areas.Admin.ViewModels.Student;
using EduHome.Models;

namespace EduHome.Areas.Admin.Mapper
{
    public class StudentMapper :Profile
    {
        public StudentMapper() {
            CreateMap<CreateStudentViewModel, Student>();
        }
    }
}
