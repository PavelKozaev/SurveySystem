using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SurveySystem.Application.Interfaces;
using SurveySystem.Contracts;
using SurveySystem.Domain.Entities;
using SurveySystem.Infrastructure.Persistence;

namespace SurveySystem.Application.Services
{
    public class InterviewService(ApplicationDbContext context, IMapper mapper) : IInterviewService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Guid> StartInterviewAsync(Guid surveyId)
        {
            var interview = new Interview
            {
                Id = Guid.NewGuid(),
                SurveyId = surveyId,
                StartedAt = DateTime.UtcNow
            };
            _context.Interviews.Add(interview);
            await _context.SaveChangesAsync();
            return interview.Id;
        }

        public async Task<QuestionWithAnswersDto?> GetCurrentQuestionAsync(Guid interviewId)
        {
            var interview = await _context.Interviews
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == interviewId);

            if (interview == null) throw new Exception("Interview not found");

            var answeredQuestionIds = await _context.Results
                .Where(r => r.InterviewId == interviewId)
                .Select(r => r.QuestionId)
                .ToListAsync();

            var nextQuestion = await _context.Questions
                .AsNoTracking()
                .Include(q => q.Answers)
                .Where(q => q.SurveyId == interview.SurveyId && !answeredQuestionIds.Contains(q.Id))
                .OrderBy(q => q.Order)
                .FirstOrDefaultAsync();

            if (nextQuestion == null)
            {
                interview.CompletedAt = DateTime.UtcNow;
                _context.Interviews.Update(interview);
                await _context.SaveChangesAsync();
                return null;
            }

            return _mapper.Map<QuestionWithAnswersDto>(nextQuestion);
        }

        public async Task<NextQuestionResponseDto> SubmitAnswerAsync(Guid interviewId, SubmitAnswerRequestDto request)
        {
            var result = new Result
            {
                Id = Guid.NewGuid(),
                InterviewId = interviewId,
                QuestionId = request.QuestionId,
                SelectedAnswerId = request.SelectedAnswerId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            var currentQuestion = await _context.Questions.FindAsync(request.QuestionId);
            var nextQuestion = await _context.Questions
                .AsNoTracking()
                .Where(q => q.SurveyId == currentQuestion.SurveyId && q.Order > currentQuestion.Order)
                .OrderBy(q => q.Order)
                .FirstOrDefaultAsync();

            return new NextQuestionResponseDto { NextQuestionId = nextQuestion?.Id };
        }
    }
}
