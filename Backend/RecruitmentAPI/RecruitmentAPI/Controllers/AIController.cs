using Microsoft.AspNetCore.Mvc;
using RecruitmentAPI.Services.Interfaces;


namespace RecruitmentAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {


        private readonly IAIService _aiService;


        public AIController(IAIService aiService)
        {
            _aiService = aiService;
        }




        [HttpPost("analyze-resume")]
        public async Task<IActionResult> AnalyzeResume(
        [FromBody] string resume)
        {

            var result =
            await _aiService.AnalyzeResumeAsync(resume);


            return Ok(new
            {
                analysis = result
            });

        }




        [HttpPost("recommend-job")]
        public async Task<IActionResult> RecommendJob(
        [FromBody] string profile)
        {


            var result =
            await _aiService.GenerateJobRecommendationAsync(
            profile,
            "Software Jobs");


            return Ok(new
            {
                recommendation = result
            });


        }




        [HttpPost("feedback")]
        public async Task<IActionResult> Feedback(
        [FromBody] string candidate)
        {


            var result =
            await _aiService.GenerateFeedbackAsync(candidate);


            return Ok(new
            {
                feedback = result
            });


        }



    }

}