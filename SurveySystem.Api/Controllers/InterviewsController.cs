using Microsoft.AspNetCore.Mvc;
using SurveySystem.Application.Exceptions;
using SurveySystem.Application.Interfaces;
using SurveySystem.Contracts;
using SurveySystem.Contracts.Responses;
using System.ComponentModel.DataAnnotations;

namespace SurveySystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InterviewsController(IInterviewService interviewService) : ControllerBase
    {
        private readonly IInterviewService _interviewService = interviewService;

        [HttpPost("/api/surveys/{surveyId}/interviews")]
        [ProducesResponseType(typeof(StartInterviewResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StartInterview(Guid surveyId)
        {
            try
            {
                var interviewId = await _interviewService.StartInterviewAsync(surveyId);
                var response = new StartInterviewResponse { InterviewId = interviewId };
                return CreatedAtAction(nameof(GetCurrentQuestion), new { interviewId }, response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{interviewId}/current-question")]
        [ProducesResponseType(typeof(QuestionWithAnswersDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SurveyCompletionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentQuestion(Guid interviewId)
        {
            try
            {
                var questionDto = await _interviewService.GetCurrentQuestionAsync(interviewId);
                return questionDto == null ? Ok(new SurveyCompletionResponse()) : Ok(questionDto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("{interviewId}/results")]
        [ProducesResponseType(typeof(NextQuestionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitAnswer(Guid interviewId, [FromBody] SubmitAnswerRequestDto request)
        {
            try
            {
                var response = await _interviewService.SubmitAnswerAsync(interviewId, request);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
