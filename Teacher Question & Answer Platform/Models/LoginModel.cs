using System.ComponentModel;

namespace TeacherStudentQAPlatform.Models
{
    public record LoginModel
    {
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

    }
}
