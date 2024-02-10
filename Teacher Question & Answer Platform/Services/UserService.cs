using Microsoft.EntityFrameworkCore;
using System.Linq;
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
                .OrderByDescending(x=>x.CreatedAt).ToListAsync();
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

        public async Task<List<QuestionOverviewModel>> GetQuestionsForUserAsync(string ?email = null, int? teacherId=null)
        {
            var questions=await _context.Questions.OrderByDescending(x=>x.CreatedAt).ToListAsync();
            var questionOverview = new List<QuestionOverviewModel>();
            if (email != null)
            {
                var user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
                if(user != null)
                    questions = questions.Where(x => x.CreatorId == user.Id).OrderByDescending(x => x.CreatedAt).ToList();
            }
            if(teacherId != null)
            {
                var tem = new List<Question>();
                foreach(var question in questions)
                {
                    var replies = await HasReplaiedByAsync(question.Id,teacherId);
                    if(replies)
                        tem.Add(question);
                }
                questions = tem;
            }
            foreach (var question in questions)
            {
                var user = await _context.Users.Where(x => x.Id == question.CreatorId).FirstOrDefaultAsync();
                questionOverview.Add(new QuestionOverviewModel
                {
                   Id = question.Id,
                   Title = question.Title,
                   CreatedOn = question.CreatedAt,
                   CreatedBy = user?.FirstName??"" +" "+user?.LastName??"",
                });
            }
            return questionOverview;
        }

        public async Task<bool>HasReplaiedByAsync(int questionId,int? teacherId)
        {
            return await _context.Answers.Where(x => x.AnswererId == teacherId && x.QuestionId == questionId).AnyAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task InsertQuestionAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

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
           return (await _context.Questions.FindAsync(id))??new Question();
        }

        public async Task InsertAnswerAsync(Answer answer)
        {
            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(Question question)
        {
            var id = question.Id;
            _context.Questions.Remove(question);
            var answers = _context.Answers.Where(x=>x.QuestionId == id);
            _context.Answers.RemoveRange(answers);
            await _context.SaveChangesAsync();
        }
    }
}
