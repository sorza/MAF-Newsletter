using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newsletter.Ai.Models;
using Newsletter.Ai.Providers.Abstractions;
using Newsletter.Core;
using Newsletter.Core.Agents.Abstractions;
using Newsletter.Core.Enums;
using Newsletter.Core.Models;
using OpenAI;
using OpenAI.Chat;

namespace Newsletter.Ai.Agents
{
    internal class NewsletterGeneratorAgent(
        ILogger<NewsletterGeneratorAgent> logger,
        [FromKeyedServices(PromptProvider.File)] IPromptProvider promptProvider)
        : IAgent<IEnumerable<Article>, string>
    {

        private const string Name = "NewsletterGeneratorAgent";
        private const string Prompt = "Gere o conteúdo da newsletter semanal com base neste JSON: ";
        private const float Temperature = 0.7f;

        public async Task<string> RunAsync(IEnumerable<Article> data, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Gerando o conteúdo da newsletter...");

            var client = new OpenAIClient(Configuration.OpenAi.ApiKey);
            var instructions = await promptProvider.GetPromptAsync(Name);

            var agent = client
                .GetChatClient(AiModels.Gpt4OMini)
                .AsAIAgent(new ChatClientAgentOptions
                {
                    Name = Name,
                    Description = "Agente especialista em gerar conteúdo para newsletter via E-mail",
                    ChatOptions = new ChatOptions
                    {
                        ModelId = AiModels.Gpt4OMini,
                        Temperature = Temperature,
                        Instructions = instructions
                    }
                });

            var prompt = $"{Prompt} {System.Text.Json.JsonSerializer.Serialize(data)}";
            var response = await agent.RunAsync<string>(prompt, cancellationToken: cancellationToken);

            logger.LogInformation("• Newsletter gerada...");
            logger.LogInformation("---");
            logger.LogInformation(response.Result);
            logger.LogInformation("---");

            return response.Result;

        }
    }
}
