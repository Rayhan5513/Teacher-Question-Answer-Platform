namespace TeacherStudentQAPlatform.Domains
{
    public class UserRoleMapping : BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
