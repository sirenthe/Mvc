namespace EduHome.Areas.Admin.ViewModels.CategoryViewModel
{
    public class DetailCategoryViewModel
    {
        public string Name { get; set; }



        public List<CourseViewModel> Courses { get; set; }

        public class CourseViewModel
        {
            public string Name { get; set; }
        }


    }
}
