using Teacher_Question___Answer_Platform.Models;
using TeacherStudentQAPlatform.Domains;
using TeacherStudentQAPlatform.Models;

namespace TeacherStudentQAPlatform.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task InsertUserAsync (User user);
        Task InsertQuestionAsync (Question question);
        Task InsertUserStudentMappintAsync(UserStudentMapping mapping);
        Task<List<QuestionOverviewModel>> GetQuestionsForUserAsync(string? email=null,int? teacherId=null);
        Task<QuestionDetailsModel> GetQuestionDetailsByIdAsync(int  id);
        Task<Question> GetQuestionByIdAsync(int id);
        Task InsertAnswerAsync(Answer answer);
        Task DeleteQuestionAsync(Question question);
    }
}
