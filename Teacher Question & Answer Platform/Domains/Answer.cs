namespace TeacherStudentQAPlatform.Domains
{
    public class Answer : BaseEntity
    {
        public int QuestionId { get; set; }
        public int AnswererId { get; set; }
        public string Comment { get; set; }
    }
}
