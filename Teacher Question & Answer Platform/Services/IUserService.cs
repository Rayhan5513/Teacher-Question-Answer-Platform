using TeacherStudentQAPlatform.Domains;
using TeacherStudentQAPlatform.Models;

namespace TeacherStudentQAPlatform.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task InsertUserAsync (User user);
        Task InsertUserStudentMappintAsync(UserStudentMapping mapping);
        Task<List<QuestionOverviewModel>> GetQuestionsForUserAsync(string email);
    }
}
