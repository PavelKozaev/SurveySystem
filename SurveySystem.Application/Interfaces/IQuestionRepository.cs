using SurveySystem.Domain.Entities;

namespace SurveySystem.Application.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question?> GetByIdAsync(Guid questionId);
        Task<Question?> GetNextQuestionAsync(Guid surveyId, IEnumerable<Guid> answeredQuestionIds);
        Task<Question?> GetNextQuestionByOrderAsync(Guid surveyId, int currentOrder);
        Task<Question?> GetByIdWithAnswersAsync(Guid questionId);
    }
}
