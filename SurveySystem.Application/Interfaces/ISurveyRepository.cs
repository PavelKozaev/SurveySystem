namespace SurveySystem.Application.Interfaces
{
    public interface ISurveyRepository
    {
        Task<bool> ExistsAsync(Guid surveyId);
    }
}
