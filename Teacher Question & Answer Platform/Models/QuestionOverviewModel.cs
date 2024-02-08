namespace TeacherStudentQAPlatform.Models
{
    public record QuestionOverviewModel :BaseModel
    {
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
    }
}
