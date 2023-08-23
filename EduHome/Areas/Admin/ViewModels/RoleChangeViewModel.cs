using EduHome.Identity;

namespace EduHome.Areas.Admin.ViewModels
{
    public class RoleChangeViewModel
    {
        public AppUser USer { get; set; }
        public IList<String> Roles { get; set; }
    }
}
