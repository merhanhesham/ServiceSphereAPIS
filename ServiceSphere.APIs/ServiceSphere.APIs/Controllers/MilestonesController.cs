using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ServiceSphere.APIs.Controllers
{
    public class MilestonesController : BaseController
    {
        private readonly string _openAIKey;

        public MilestonesController(IConfiguration configuration)
        {
            _openAIKey = configuration["OpenAI:ApiKey"]; // Ensure your API key is under OpenAI:ApiKey in your appsettings.json
        }

        [HttpPost]
        public async Task<IActionResult> GenerateMainMilestones([FromBody] string userInput)
        {
            try
            {
                string mainMilestones = await GenerateMainMilestones(userInput, _openAIKey);
                return Ok(mainMilestones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to generate main milestones: {ex.Message}");
            }
        }

        private async Task<string> GenerateMainMilestones(string userInput, string apiKey)
        {
            string promptText = $"Generate main milestones that consist of multiple actionable tasks suitable for parallel work by a team of five members, excluding tasks related to team recruitment or contracting:\n- {userInput}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var requestData = new
                {
                    prompt = promptText,
                    model = "gpt-3.5-turbo-instruct",
                    temperature = 0.5,
                    max_tokens = 150
                };

                var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://api.openai.com/v1/completions", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to generate main milestones. Status code: {response.StatusCode}. Response content: {responseContent}");
                }
            }
        }
    }
}
