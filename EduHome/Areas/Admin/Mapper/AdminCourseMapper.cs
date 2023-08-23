using AutoMapper;
using EduHome.Areas.Admin.ViewModels.CourseViewModel;
using EduHome.Areas.Admin.ViewModels.TeacherViewModel;
using EduHome.Models;
using EduHome.ViewModels;

namespace EduHome.Areas.Admin.Mapper
{
	public class AdminCourseMapper :Profile
	{
		public AdminCourseMapper() {
		
            CreateMap<Course, DeleteCourseViewModel>()
			 .ForMember(dest => dest.Img, opt => opt.MapFrom(src => src.Image));
			CreateMap<CreateCourseViewModel, Course>()
					  .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Img));
			CreateMap<Course, DetailCourseViewModel>()
              .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.courseCategories.Select(cc => cc.Category).ToList()));




        }
	}
}
