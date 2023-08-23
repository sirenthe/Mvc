using AutoMapper;
using EduHome.Models;
using EduHome.ViewModels;

namespace EduHome.Mappers
{
    public class BlogMapper :Profile
    {
        public BlogMapper() {
            CreateMap<Blog, BlogViewModel>().ReverseMap();
            CreateMap<Blog, DetailBlogViewModel>().ReverseMap();
        }
    }
}
