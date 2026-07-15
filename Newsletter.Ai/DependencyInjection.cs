using Microsoft.Extensions.DependencyInjection;
using Newsletter.Ai.Agents;
using Newsletter.Ai.Providers;
using Newsletter.Ai.Providers.Abstractions;
using Newsletter.Core.Agents.Abstractions;
using Newsletter.Core.Enums;
using Newsletter.Core.Models;

namespace Newsletter.Ai
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAgents(this IServiceCollection services) 
        { 
            services.AddKeyedTransient<IAgent<IEnumerable<Article>, string>, NewsletterGeneratorAgent>(AgentType.NewsletterGenerator);
            services.AddKeyedTransient<IAgent<IEnumerable<Article>, string>, TitleGeneratorAgent>(AgentType.TitleGenerator);
            services.AddKeyedTransient<IPromptProvider, FilePromptProvider>(PromptProvider.File);

            return services;
        }
    }
}
