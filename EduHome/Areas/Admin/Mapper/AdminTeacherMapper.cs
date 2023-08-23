using AutoMapper;
using EduHome.Areas.Admin.ViewModels.TeacherViewModel;
using EduHome.Models;
using EduHome.ViewModels;
using static EduHome.ViewModels.DetailTeacherViewModel;

namespace EduHome.Areas.Admin.Mapper
{
    public class AdminTeacherMapper:Profile 
    {
        public AdminTeacherMapper() {
            CreateMap<Teacher, DeleteTeacherViewModel>().ForMember(dest => dest.Img, opt => opt.MapFrom(src => src.Image));
			CreateMap<CreateTeacherViewModel, Teacher>();

			CreateMap<Teacher, DetailTeacherViewModel>()
          .ForMember(dest => dest.SocialMedias, opt => opt.MapFrom(src => src.SocialMedias))
          .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.TeacherSkills.Select(ts => ts.Skill)))
          .ReverseMap();
            CreateMap<Teacher, DetailTeacherViewModel>()
           .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.TeacherSkills.Select(ts => ts.Skill)))
           .ReverseMap(); 

            CreateMap<SocialMedia, SocialMediaViewModel>().ReverseMap();
            CreateMap<Skill, SkillViewModel>();
        }
    }
}
