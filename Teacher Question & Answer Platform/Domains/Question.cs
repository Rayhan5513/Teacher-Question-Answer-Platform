namespace TeacherStudentQAPlatform.Domains
{
    public class Question : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
    }
}
