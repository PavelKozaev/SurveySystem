using Microsoft.EntityFrameworkCore;
using SurveySystem.Application.Interfaces;
using SurveySystem.Infrastructure.Persistence;

namespace SurveySystem.Infrastructure.Repositories
{
    public class SurveyRepository(ApplicationDbContext context) : ISurveyRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<bool> ExistsAsync(Guid surveyId)
        {
            return await _context.Surveys.AnyAsync(s => s.Id == surveyId);
        }
    }
}
