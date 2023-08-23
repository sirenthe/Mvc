namespace EduHome.ViewModels
{
    public class DetailTeacherViewModel
    {
       
            public string Image { get; set; }
            public string Name { get; set; }
            public string Occupation { get; set; }
            public string Description { get; set; }
            public string Degree { get; set; }
		public string Experinece { get; set; }
		public string Hobbies { get; set; }
            public string Faculty { get; set; }
            public string Skype { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }

            public List<SocialMediaViewModel> SocialMedias { get; set; }
            public List<SkillViewModel> Skills { get; set; }
        

        public class SocialMediaViewModel
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }

        public class SkillViewModel
        {
            public string Name { get; set; }
         
        }

    }
}
