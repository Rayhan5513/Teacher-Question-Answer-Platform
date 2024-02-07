using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Teacher_Question___Answer_Platform.Database
{
    public class UserInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserInfoId { get; set; }

        [Required(ErrorMessage = "Instute name is required")]
        [EmailAddress]
        public string? InstuteName { get; set; }

        [Required(ErrorMessage = "Instute ID is required")]
        public string? InstuteId { get; set; }


    }
}
