using System.ComponentModel.DataAnnotations.Schema;

namespace Teacher_Question___Answer_Platform.Database
{
    public class AnswerTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }
        public string? Answer { get; set; }
        public string? ReplyPersonId { get; set; }
    }
}
