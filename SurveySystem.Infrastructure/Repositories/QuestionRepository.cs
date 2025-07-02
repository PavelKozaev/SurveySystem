using Microsoft.EntityFrameworkCore;
using SurveySystem.Application.Interfaces;
using SurveySystem.Domain.Entities;
using SurveySystem.Infrastructure.Persistence;

namespace SurveySystem.Infrastructure.Repositories
{
    public class QuestionRepository(ApplicationDbContext context) : IQuestionRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Question?> GetByIdAsync(Guid questionId)
        {
            return await _context.Questions
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

        public async Task<Question?> GetNextQuestionAsync(Guid surveyId, IEnumerable<Guid> answeredQuestionIds)
        {
            return await _context.Questions
                .AsNoTracking()
                .Include(q => q.Answers)
                .Where(q => q.SurveyId == surveyId && !answeredQuestionIds.Contains(q.Id))
                .OrderBy(q => q.Order)
                .FirstOrDefaultAsync();
        }

        public async Task<Question?> GetNextQuestionByOrderAsync(Guid surveyId, int currentOrder)
        {
            return await _context.Questions
                .AsNoTracking()
                .Where(q => q.SurveyId == surveyId && q.Order > currentOrder)
                .OrderBy(q => q.Order)
                .FirstOrDefaultAsync();
        }
    }
}
