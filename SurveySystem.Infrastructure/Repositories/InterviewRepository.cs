using Microsoft.EntityFrameworkCore;
using SurveySystem.Application.Interfaces;
using SurveySystem.Domain.Entities;
using SurveySystem.Infrastructure.Persistence;

namespace SurveySystem.Infrastructure.Repositories
{
    public class InterviewRepository(ApplicationDbContext context) : IInterviewRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Interview?> GetByIdAsync(Guid interviewId)
        {
            return await _context.Interviews.FirstOrDefaultAsync(i => i.Id == interviewId);
        }

        public async Task AddAsync(Interview interview)
        {
            await _context.Interviews.AddAsync(interview);
        }

        public void Update(Interview interview)
        {
            _context.Interviews.Update(interview);
        }
    }
}
