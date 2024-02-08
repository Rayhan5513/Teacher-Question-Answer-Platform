namespace TeacherStudentQAPlatform.Rules
{
    public interface IRule
    {
        Task<bool> Check();
    }
}
