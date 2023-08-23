using AutoMapper;
using EduHome.Areas.Admin.ViewModels;
using EduHome.Identity;

namespace EduHome.Areas.Admin.Mapper
{
    public class UserMapper :Profile
    {
     public UserMapper() {
            CreateMap< AppUser, UserViewModel>().ReverseMap();
        }    
    }
}
