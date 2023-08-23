using System.ComponentModel.DataAnnotations;
using EduHome.Identity;
using EduHome.Models.common;

namespace EduHome.Models
{
    public class Subscriber :BaseEntity
    {

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool IsSubscribed { get; set; }  = false;
    }
}
