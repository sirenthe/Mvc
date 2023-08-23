using AutoMapper;
using EduHome.Models;
using EduHome.ViewModels;

namespace EduHome.Mappers
{
	public class TeacherMapper :Profile 
	{
		public TeacherMapper() {
			CreateMap<Teacher, TeacherViewModel>().ReverseMap();
		}
	}
}
