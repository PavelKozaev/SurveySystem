using SurveySystem.Contracts;

namespace SurveySystem.Application.Interfaces
{
    public interface IInterviewService
    {
        Task<Guid> StartInterviewAsync(Guid surveyId);
        Task<QuestionWithAnswersDto?> GetCurrentQuestionAsync(Guid interviewId);
        Task<NextQuestionResponseDto> SubmitAnswerAsync(Guid interviewId, SubmitAnswerRequestDto request);
    }
}
