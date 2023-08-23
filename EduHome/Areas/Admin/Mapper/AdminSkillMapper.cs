using AutoMapper;
using EduHome.Areas.Admin.ViewModels.SkillViewModel;
using EduHome.Models;
using EduHome.ViewModels;
using static EduHome.ViewModels.DetailTeacherViewModel;

namespace EduHome.Areas.Admin.Mapper
   
{
    public class AdminSkillMapper : Profile
    {
        public AdminSkillMapper() {

            CreateMap<Skill, DetailSkillViewModel>()
        .ForMember(dest => dest.Teachers, opt => opt.MapFrom(src => src.TeacherSkills.Select(ts => ts.Teacher).ToList()))
        .ReverseMap();

            CreateMap<Teacher, DetailSkillViewModel.TeacherViewModel>();

        }
    }
}
