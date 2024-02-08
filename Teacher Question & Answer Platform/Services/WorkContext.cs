using TeacherStudentQAPlatform.Domains;

namespace TeacherStudentQAPlatform.Services
{
    public class WorkContext : IWorkContext
    {
        private User User { get; set; }
        public async Task<User> GetCurrentUserAsync()
        {
            return User;
        }

        public void SetCurrentUser(User user)
        {
            User = user;
        }

        public void SetCurrentUser()
        {
            throw new NotImplementedException();
        }
    }
}
