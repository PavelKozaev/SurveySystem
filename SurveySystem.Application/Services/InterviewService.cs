using AutoMapper;
using SurveySystem.Application.Interfaces;
using SurveySystem.Contracts;
using SurveySystem.Domain.Entities;

namespace SurveySystem.Application.Services
{
    public class InterviewService(
        IInterviewRepository interviewRepository,
        IQuestionRepository questionRepository,
        IResultRepository resultRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IInterviewService
    {
        private readonly IInterviewRepository _interviewRepository = interviewRepository;
        private readonly IQuestionRepository _questionRepository = questionRepository;
        private readonly IResultRepository _resultRepository = resultRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Guid> StartInterviewAsync(Guid surveyId)
        {
            var interview = new Interview
            {
                Id = Guid.NewGuid(),
                SurveyId = surveyId,
                StartedAt = DateTime.UtcNow
            };
            await _interviewRepository.AddAsync(interview);
            await _unitOfWork.SaveChangesAsync(); 
            return interview.Id;
        }

        public async Task<QuestionWithAnswersDto?> GetCurrentQuestionAsync(Guid interviewId)
        {
            var interview = await _interviewRepository.GetByIdAsync(interviewId)
                ?? throw new Exception($"Interview with id {interviewId} not found."); 

            var answeredQuestionIds = await _resultRepository.GetAnsweredQuestionIdsAsync(interviewId);

            var nextQuestion = await _questionRepository.GetNextQuestionAsync(interview.SurveyId, answeredQuestionIds);

            if (nextQuestion == null)
            {
                interview.CompletedAt = DateTime.UtcNow;
                _interviewRepository.Update(interview);
                await _unitOfWork.SaveChangesAsync();
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
            await _resultRepository.AddAsync(result);

            var currentQuestion = await _questionRepository.GetByIdAsync(request.QuestionId)
                ?? throw new Exception($"Question with id {request.QuestionId} not found.");

            var nextQuestion = await _questionRepository.GetNextQuestionByOrderAsync(currentQuestion.SurveyId, currentQuestion.Order);

            await _unitOfWork.SaveChangesAsync();

            return new NextQuestionResponseDto { NextQuestionId = nextQuestion?.Id };
        }
    }
}
