namespace SurveySystem.Domain.Entities
{
    public class Result
    {
        public Guid Id { get; set; }
        public Guid InterviewId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid SelectedAnswerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}