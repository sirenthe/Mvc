using AutoMapper;
using EduHome.Areas.Admin.ViewModels.CourseViewModel;
using EduHome.Areas.Admin.ViewModels.EventViewModel;
using EduHome.Areas.Admin.ViewModels.TeacherViewModel;
using EduHome.Models;

namespace EduHome.Areas.Admin.Mapper
{
    public class AdminEventMapper :Profile
    {
        public AdminEventMapper() {

            CreateMap<Event, DeleteEventViewModel>();
            CreateMap<Event, DetailEventViewModel>()
          .ForMember(dest => dest.Speakers, opt => opt.MapFrom(src => src.eventSpeakers.Select(cc => cc.Speaker).ToList()));
            CreateMap<CreateEventViewModel, Event>();
				
		}
    }
}
