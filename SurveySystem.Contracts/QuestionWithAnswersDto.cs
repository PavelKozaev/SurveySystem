namespace SurveySystem.Contracts
{
    public class QuestionWithAnswersDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<AnswerDto> AnswerOptions { get; set; } = [];
    }
}
