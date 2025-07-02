using SurveySystem.Domain.Entities;

namespace SurveySystem.Application.Interfaces
{
    public interface IResultRepository
    {
        Task<List<Guid>> GetAnsweredQuestionIdsAsync(Guid interviewId);
        Task AddAsync(Result result);
        Task<bool> ExistsAsync(Guid interviewId, Guid questionId);
    }
}
