namespace TeacherStudentQAPlatform.Rules
{
    public class DeleteRule : IRule
    {
        private int _userId;
        private int _questionId;

        public DeleteRule(int userId, int questionId)
        {
            _userId = userId;
            _questionId = questionId;
        }
        public async Task<bool> Check()
        {
            return true;
        }
    }
}
