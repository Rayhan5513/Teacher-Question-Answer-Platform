using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teacher_Question___Answer_Platform.Database
{
    public class RoleTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "select option")]
        public string? RoleName { get; set; }


    }
}
