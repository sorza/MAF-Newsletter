using Microsoft.Extensions.Logging;
using Newsletter.Core;
using Newsletter.Core.Agents.Abstractions;
using Newsletter.Core.Models;
using OpenAI;

namespace Newsletter.Ai.Agents
{
    public class TitleGeneratorAgent(ILogger<TitleGeneratorAgent> logger) : IAgent<IEnumerable<Article>, string>
    {

        private const string Name = "TitleGeneratorAgent";
        private const string Prompt = "Gere um título para a newsletter com base nos seguintes artigos:\n{0}\n\nO título deve ser curto, chamativo e resumir o conteúdo dos artigos.";
        private const float Temperature = 0.7f;

        public async Task<string> RunAsync(IEnumerable<Article> data, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Gerando o título da newsletter...");

            var client = new OpenAIClient(Configuration.OpenAi.ApiKey);
            var instructions = await promptProvider.GetPromptAsync(Name);
        }
    }
}
