using CodeInclusionScanner.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class Gpt3Service
{
    private readonly string _apiKey = "";
    private readonly HttpClient _httpClient;

    public Gpt3Service()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GenerateAsync(string promptInputData)
    {
        try
        {
            var apiUrl = "https://api.openai.com/v1/engines/davinci/completions";
            string prompt = "Identify the non-inclusive term in the given text snippet and suggest alternative, more inclusive terms.\n\n{\n}";
            string promptRequestData = "{\"prompt\":\"" + prompt + "\n" + promptInputData + "\"}";


            var requestData = new
            {
                prompt = promptRequestData,
                max_tokens = 300, // Adjust the number of tokens as needed.
            };

            var requestContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsync(apiUrl, requestContent);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<AIResponseModel>(responseBody).Choices[0].Text;
                return responseModel;
            }
            else
            {
                throw new Exception($"GPT-3 API request failed with status code {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., network errors, API errors) here.
            throw ex;
        }
    }

    public static int GetBlackManCount()
    {
        return 1;
    }
}
