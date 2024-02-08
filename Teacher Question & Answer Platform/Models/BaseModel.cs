namespace TeacherStudentQAPlatform.Models
{
    public record BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Dictionary<string, string> CustomProperties { get; set; }
    }
}
