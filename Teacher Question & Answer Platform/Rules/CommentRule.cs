using TeacherStudentQAPlatform.Domains;

namespace TeacherStudentQAPlatform.Rules
{
    public class CommentRule : IRule
    {
        private int _userId;
        private int _questionId;

        public CommentRule(int userId, int questionId)
        {
            _userId = userId;
            _questionId = questionId;
        }
        public async Task<bool> Check()
        {
            // TODO : task-1
            return true;
        }
    }
}
