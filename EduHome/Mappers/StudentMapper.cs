using AutoMapper;
using EduHome.Models;
using EduHome.ViewModels;

namespace EduHome.Mappers
{
    public class StudentMapper :Profile
    {
        public StudentMapper() {
            CreateMap<Student, StudentViewModel>().ReverseMap();
        }
    }
}
