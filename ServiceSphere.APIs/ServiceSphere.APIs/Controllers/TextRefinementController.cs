using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceSphere.APIs.Controllers
{
    public class TextRefinementController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly string _openAIKey;  // Store the API key

        public TextRefinementController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _openAIKey = configuration["OpenAI:ApiKey"];  // Retrieve the API key from appsettings.json
        }

        [HttpPost("RefineText")]
        public async Task<IActionResult> Post([FromBody] string userInput)
        {
            string prompt = $@"
                Project Title: Repair of Faulty Air Conditioning Unit

                Project Description:
                The customer reports that their air conditioning unit is not working properly, making unusual noises and failing to cool the room. Describe the necessary steps to diagnose and resolve these issues based on the following user report:
                {userInput}

                Please enhance the following text to ensure it is both formal and technically accurate.";

            var requestBody = new
            {
                prompt = prompt,
                model = "gpt-3.5-turbo-instruct",
                max_tokens = 300,  // Allow more tokens for a more complete response
                temperature = 0.5  // Adjust as necessary to control creativity
            };

            string serializedBody = JsonSerializer.Serialize(requestBody);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAIKey);  // Use the API key from configuration
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(serializedBody, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/completions", content);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return Ok(responseContent);
            }
            else
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
    }
}
