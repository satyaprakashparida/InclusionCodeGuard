using System.Linq;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using Azure;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using CodeInclusionScanner.Models;

namespace CodeInclusionScanner.Services
{
    public class AzureOpenAIService
    {

        private readonly OpenAIClient _openAIClient;
        private ChatCompletionsOptions options;

        public AzureOpenAIService()
        {
            this._openAIClient = new OpenAIClient(
                        new Uri("https://cap-gpt.openai.azure.com/"),
                        new AzureKeyCredential(""));
            this.options = new ChatCompletionsOptions()
            {
                Temperature = (float)0.1,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };
        }
        public async Task<string> ExtractNonInclusiveTerms(string selectedCode)
        {
            try
            {

                var systemMessage = @"
                Identify the non-inclusive term in the given text snippet and suggest alternative, more inclusive terms.";
                var prompt = JsonConvert.SerializeObject(selectedCode);
                this.options.ResetOptions();

                this.options.AddMessage(ChatRole.System, systemMessage);
                this.options.AddMessage(ChatRole.User, prompt);

                Response<ChatCompletions> responseWithoutStream = await _openAIClient.GetChatCompletionsAsync(
                    "Turbo", options);

                var completions = responseWithoutStream.Value;                

                return completions.Choices.FirstOrDefault().Message.Content;
            }
            catch (Exception ex)
            {                
                return null;
            }
        }
    }
}
