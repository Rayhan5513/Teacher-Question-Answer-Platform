using TeacherStudentQAPlatform.Domains;

namespace TeacherStudentQAPlatform.Services
{
    public interface IWorkContext
    {
        Task<User> GetCurrentUserAsync();
        void SetCurrentUser(User user);
        
    }
}
