namespace SurveySystem.Domain.Entities
{
    public class Interview
    {
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public ICollection<Result> Results { get; set; } = new List<Result>();
    }
}
