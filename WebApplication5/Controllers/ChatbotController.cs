using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("api/AI")]
    public class ChatbotController : ControllerBase
    {
        [HttpGet("ask")]
        public async Task<IActionResult> GenerateAI(string prompt, Kernel kernel)
        {
            try
            {
                var resultStream = kernel.InvokePromptStreamingAsync<string>(prompt);

                var fullResult = new StringBuilder();

                await foreach (var part in resultStream)
                {

                    var splitParts = part.Split(new[] { "\n", ".", ":", "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var splitPart in splitParts)
                    {
                        fullResult.Append(splitPart.Trim() + " ");
                    }
                }

                return Ok(new { response = fullResult.ToString().Trim() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
