using System.ComponentModel.DataAnnotations.Schema;

namespace Teacher_Question___Answer_Platform.Database
{
    public class QuestionTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        public string? UQuestionTitle { get; set; }
        public string? QuestionDescription { get; set; }
        public string? AskerId { get; set; }
    }
}
