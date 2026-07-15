using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using System.Text.Json;
using Newsletter.Ai.Models;
using Newsletter.Ai.Providers.Abstractions;
using Newsletter.Core;
using Newsletter.Core.Agents.Abstractions;
using Newsletter.Core.Enums;
using Newsletter.Core.Models;

namespace Newsletter.Ai.Agents
{
    public class TitleGeneratorAgent(
        ILogger<TitleGeneratorAgent> logger,
        [FromKeyedServices(PromptProvider.File)]IPromptProvider promptProvider)
        : IAgent<IEnumerable<Article>, string>
    {

        private const string Name = "TitleGeneratorAgent";
        private const string Prompt = "Gere um título para a newsletter semanal com base neste JSON: ";
        private const float Temperature = 0.7f;

        public async Task<string> RunAsync(IEnumerable<Article> data, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Gerando o título da newsletter...");

            var client = new OpenAIClient(Configuration.OpenAi.ApiKey);
            var instructions = await promptProvider.GetPromptAsync(Name);

            var agent = client
                .GetChatClient(AiModels.Gpt4OMini)
                .AsAIAgent(new ChatClientAgentOptions
                {
                    Name = Name,
                    Description = "Agente especialista em gerar título para newsletter",
                    ChatOptions = new ChatOptions
                    {
                        ModelId = AiModels.Gpt4OMini,
                        Temperature = Temperature,
                        Instructions = instructions
                    }
                });

            var prompt = $"{Prompt} {JsonSerializer.Serialize(data)}";
            var response = await agent.RunAsync<string>(prompt, cancellationToken: cancellationToken);

            logger.LogInformation("Título gerado com sucesso: {Title}", response.Result);

            return response.Result;
        }
    }
}
