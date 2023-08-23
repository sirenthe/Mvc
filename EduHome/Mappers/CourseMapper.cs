using AutoMapper;
using EduHome.Models;
using EduHome.ViewModels;

namespace EduHome.Mappers
{
    public class CourseMapper :Profile
    {
        public CourseMapper()
        {
        CreateMap<Course, CourseViewModel>().ReverseMap();
            CreateMap<Course, ReadmoreViewModel>().ForMember(x => x.Categories, opt => opt.MapFrom(src => src.courseCategories.Select(es => es.Category))).ReverseMap();


        }
    }
}
