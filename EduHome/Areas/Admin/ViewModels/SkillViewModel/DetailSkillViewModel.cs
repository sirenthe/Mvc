namespace EduHome.Areas.Admin.ViewModels.SkillViewModel
{
    public class DetailSkillViewModel
    {
      


            public string Name { get; set; }
     
            public double Percent { get; set; }


            public List<TeacherViewModel> Teachers { get; set; }


          

            public class TeacherViewModel
            {
                public string Name { get; set; }
            }

        }
    }

