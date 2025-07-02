namespace SurveySystem.Domain.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}