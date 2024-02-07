using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;



namespace Teacher_Question___Answer_Platform.Database
{
    public class UserTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }


        [Required(ErrorMessage = "Name is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

    }
}
