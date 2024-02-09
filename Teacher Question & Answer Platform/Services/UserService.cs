using Microsoft.EntityFrameworkCore;
using Teacher_Question___Answer_Platform.Models;
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

        public async Task<string> GetUserNameByIdAsync(int id)
        {
            var user =await  _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(user == null)
            {
                return "";
            }
            return user.FirstName + " " +user.LastName;
        }

        public async Task<QuestionDetailsModel> GetQuestionDetailsByIdAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if(question == null)
            {
                return new QuestionDetailsModel();
            }
            var answers =await _context.Answers.Where(x => x.QuestionId == id)
                .OrderBy(x=>x.CreatedAt).ToListAsync();
            var model = new QuestionDetailsModel();
            model.Title =  question.Title;
            model.Description = question.Description;
            model.CreatedBy = await GetUserNameByIdAsync(question.CreatorId);
            model.Id = question.Id;
            foreach(var answer in answers)
            {
                model.Answers.Add(new AnswerModel
                {
                    Answer = answer.Comment,
                    AnsweredBy = await GetUserNameByIdAsync(answer.AnswererId)
                });
            }
            return model;
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

        public async Task InsertQuestionAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
        }

        public async Task InsertUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task InsertUserStudentMappintAsync(UserStudentMapping mapping)
        {
            await _context.UserStudentMappings.AddAsync(mapping);
            await _context.SaveChangesAsync();
        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
           return await _context.Questions.FindAsync(id);
        }

        public async Task InsertAnswerAsync(Answer answer)
        {
            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();
        }
    }
}
