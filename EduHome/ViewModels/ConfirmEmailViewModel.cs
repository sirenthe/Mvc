using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels
{
    public class ConfirmEmailViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }   
    }
}
