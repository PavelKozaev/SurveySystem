using AutoMapper;
using SurveySystem.Application.Exceptions;
using SurveySystem.Application.Interfaces;
using SurveySystem.Contracts;
using SurveySystem.Domain.Entities;
using System.ComponentModel.DataAnnotations; 

namespace SurveySystem.Application.Services
{
    public class InterviewService(
        ISurveyRepository surveyRepository,
        IInterviewRepository interviewRepository,
        IQuestionRepository questionRepository,
        IResultRepository resultRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IInterviewService
    {
        private readonly ISurveyRepository _surveyRepository = surveyRepository; 
        private readonly IInterviewRepository _interviewRepository = interviewRepository;
        private readonly IQuestionRepository _questionRepository = questionRepository;
        private readonly IResultRepository _resultRepository = resultRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Guid> StartInterviewAsync(Guid surveyId)
        {
            if (!await _surveyRepository.ExistsAsync(surveyId))
            {
                throw new NotFoundException($"Survey with id '{surveyId}' not found.");
            }

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
                ?? throw new NotFoundException($"Interview with id '{interviewId}' not found.");

            if (interview.CompletedAt.HasValue)
            {
                return null;
            }

            var answeredQuestionIds = await _resultRepository.GetAnsweredQuestionIdsAsync(interviewId);
            var nextQuestion = await _questionRepository.GetNextQuestionAsync(interview.SurveyId, answeredQuestionIds);

            if (nextQuestion is null)
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
            var interview = await _interviewRepository.GetByIdAsync(interviewId)
                ?? throw new NotFoundException($"Interview with id '{interviewId}' not found.");

            if (interview.CompletedAt.HasValue)
            {
                throw new ValidationException("This interview has already been completed.");
            }

            if (await _resultRepository.ExistsAsync(interviewId, request.QuestionId))
            {
                throw new ValidationException($"An answer for question '{request.QuestionId}' has already been submitted for this interview.");
            }

            var currentQuestion = await _questionRepository.GetByIdWithAnswersAsync(request.QuestionId)
                ?? throw new NotFoundException($"Question with id '{request.QuestionId}' not found.");

            if (currentQuestion.Answers.All(a => a.Id != request.SelectedAnswerId))
            {
                throw new ValidationException($"Answer '{request.SelectedAnswerId}' does not belong to question '{request.QuestionId}'.");
            }

            var result = new Result
            {
                Id = Guid.NewGuid(),
                InterviewId = interviewId,
                QuestionId = request.QuestionId,
                SelectedAnswerId = request.SelectedAnswerId,
                CreatedAt = DateTime.UtcNow
            };
            await _resultRepository.AddAsync(result);

            var nextQuestion = await _questionRepository.GetNextQuestionByOrderAsync(currentQuestion.SurveyId, currentQuestion.Order);

            await _unitOfWork.SaveChangesAsync();

            return new NextQuestionResponseDto { NextQuestionId = nextQuestion?.Id };
        }
    }
}