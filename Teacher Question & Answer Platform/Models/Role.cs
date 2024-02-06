using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teacher_Question___Answer_Platform.Models
{
    public class Role
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

       // [Required(ErrorMessage = "RollId is re")]
        [EmailAddress]
        public string? RoleName { get; set; }

    }
}
