using AutoMapper;
using EduHome.Identity;
using EduHome.Models;
using EduHome.ViewModels;

namespace EduHome.Mappers
{
    public class EventMapper :Profile
    {
        public EventMapper()
        {
            CreateMap<Event, EventViewModel>().ReverseMap();
            CreateMap<Event, DetailEventViewModel>().ForMember(x=>x.Speakers, opt => opt.MapFrom(src => src.eventSpeakers.Select(es=>es.Speaker))).ReverseMap();

        
        }
    }
}
