using TeacherStudentQAPlatform.Models;

namespace Teacher_Question___Answer_Platform.Models
{
    public record AnswerModel :BaseModel
    {
        public string AnsweredBy { get; set; }
        public string Answer { get; set; }
    }
}
