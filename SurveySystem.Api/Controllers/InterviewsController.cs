using Microsoft.AspNetCore.Mvc;
using SurveySystem.Application.Interfaces;
using SurveySystem.Contracts;

namespace SurveySystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InterviewsController : ControllerBase
    {
        private readonly IInterviewService _interviewService;

        public InterviewsController(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        [HttpPost("/api/surveys/{surveyId}/interviews")]
        public async Task<IActionResult> StartInterview(Guid surveyId)
        {
            var interviewId = await _interviewService.StartInterviewAsync(surveyId);
            return Ok(new { interviewId });
        }

        [HttpGet("{interviewId}/current-question")]
        public async Task<IActionResult> GetCurrentQuestion(Guid interviewId)
        {
            var questionDto = await _interviewService.GetCurrentQuestionAsync(interviewId);
            if (questionDto == null)
            {
                return Ok(new { isCompleted = true }); 
            }
            return Ok(questionDto);
        }

        [HttpPost("{interviewId}/results")]
        public async Task<IActionResult> SubmitAnswer(Guid interviewId, [FromBody] SubmitAnswerRequestDto request)
        {
            var response = await _interviewService.SubmitAnswerAsync(interviewId, request);
            return Ok(response);
        }
    }
}
