using Microsoft.EntityFrameworkCore;
using TeacherStudentQAPlatform.Data;
using TeacherStudentQAPlatform.Domains;
using TeacherStudentQAPlatform.Models;

namespace TeacherStudentQAPlatform.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<List<QuestionOverviewModel>> GetQuestionsForUserAsync(string email)
        {
            var questions=await _context.Questions.ToListAsync();
            var questionOverview = new List<QuestionOverviewModel>();
            foreach (var question in questions)
            {
                var user = await _context.Users.Where(x => x.Id == question.CreatorId).FirstOrDefaultAsync();
                questionOverview.Add(new QuestionOverviewModel
                {
                   Title = question.Title,
                   CreatedOn = question.CreatedAt,
                   CreatedBy = user?.FirstName??"" +" "+user?.LastName??"",
                });
            }
            return questionOverview;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task InsertUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task InsertUserStudentMappintAsync(UserStudentMapping mapping)
        {
            await _context.UserStudentMappings.AddAsync(mapping);
        }
    }
}
