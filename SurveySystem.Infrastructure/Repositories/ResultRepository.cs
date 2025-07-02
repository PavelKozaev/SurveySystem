using Microsoft.EntityFrameworkCore;
using SurveySystem.Application.Interfaces;
using SurveySystem.Domain.Entities;
using SurveySystem.Infrastructure.Persistence;

namespace SurveySystem.Infrastructure.Repositories
{
    public class ResultRepository(ApplicationDbContext context) : IResultRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<Guid>> GetAnsweredQuestionIdsAsync(Guid interviewId)
        {
            return await _context.Results
                .Where(r => r.InterviewId == interviewId)
                .Select(r => r.QuestionId)
                .ToListAsync();
        }

        public async Task AddAsync(Result result)
        {
            await _context.Results.AddAsync(result);
        }

        public async Task<bool> ExistsAsync(Guid interviewId, Guid questionId)
        {
            return await _context.Results
                .AnyAsync(r => r.InterviewId == interviewId && r.QuestionId == questionId);
        }
    }
}
