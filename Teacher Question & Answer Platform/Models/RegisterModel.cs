using System.ComponentModel;

namespace TeacherStudentQAPlatform.Models
{
    public record RegisterModel
    {
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("LastName")]
        public string LastName { get; set; }

        [DisplayName("FirstName")]
        public string FirstName { get; set; }
        [DisplayName("Register as Student")]
        public bool IsStudent { get; set; }

        [DisplayName("Institute name")]
        public string? InstituteName { get; set; }

        [DisplayName("Versity id card info")]
        public string? VersityIdCard { get; set; }
    }
}
