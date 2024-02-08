namespace TeacherStudentQAPlatform.Domains
{
    public class UserStudentMapping : BaseEntity
    {
        public int UserId { get; set; }
        public string InstituteName { get; set; }
        public string VersityIdCard { get; set; }
    }
}
