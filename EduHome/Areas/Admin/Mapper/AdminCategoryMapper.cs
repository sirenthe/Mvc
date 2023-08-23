using AutoMapper;
using EduHome.Areas.Admin.ViewModels.CategoryViewModel;
using EduHome.Areas.Admin.ViewModels.SkillViewModel;
using EduHome.Models;

namespace EduHome.Areas.Admin.Mapper
{
    public class AdminCategoryMapper :Profile
    {
        public AdminCategoryMapper()
        {

            CreateMap<Category, DetailCategoryViewModel>()
        .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.courseCategories.Select(ts => ts.Course).ToList()))
        .ReverseMap();

            CreateMap<Course, DetailCategoryViewModel.CourseViewModel>();

        }
    }
}
