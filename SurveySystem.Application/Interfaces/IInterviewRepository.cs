using SurveySystem.Domain.Entities;

namespace SurveySystem.Application.Interfaces
{
    public interface IInterviewRepository
    {
        Task<Interview?> GetByIdAsync(Guid interviewId);
        Task AddAsync(Interview interview);
        void Update(Interview interview);
    }
}
