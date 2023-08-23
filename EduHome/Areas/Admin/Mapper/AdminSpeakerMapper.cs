using AutoMapper;
using EduHome.Areas.Admin.ViewModels.EventViewModel;
using EduHome.Areas.Admin.ViewModels.SpeakerViewModel;
using EduHome.Areas.Admin.ViewModels.TeacherViewModel;
using EduHome.Models;

namespace EduHome.Areas.Admin.Mapper
{
    public class AdminSpeakerMapper :Profile
    {
        public AdminSpeakerMapper() {
            CreateMap<Speaker, DeleteSpeakerViewModel>();
            CreateMap<Speaker, DetailSpeakerViewModel>()
    .ForMember(dest => dest.Events, opt => opt.MapFrom(src => src.eventSpeakers.Select(cc => cc.Event).ToList()));
        }
    }
}
