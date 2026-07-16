using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newsletter.Core.Agents.Abstractions;
using Newsletter.Core.Enums;
using Newsletter.Core.Models;
using Newsletter.Core.Repositories.Abstractions;
using Newsletter.Core.Services.Abstractions;

namespace Newsletter.Infra.Services
{
    internal class NewsletterService(
        ILogger<NewsletterService> logger,
        IArticleRepository articleRepository,
        [FromKeyedServices(AgentType.TitleGenerator)] IAgent<IEnumerable<Article>, string> titleGeneratorAgent,
        [FromKeyedServices(AgentType.NewsletterGenerator)] IAgent<IEnumerable<Article>, string> newsletterGeneratorAgent,
        ISubscriberRepository subscriberRepository,
        IEmailService emailService) 
        : INewsletterService
    {
        public async Task SendAsync(CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Recuperando os posts da semana...");
            var posts = await articleRepository.GetFromLastWeekAsync(cancellationToken);

            if (!posts.Any())
                return;

            logger.LogInformation("Gerando o título do newsletter...");
            var subject = await titleGeneratorAgent.RunAsync(posts, cancellationToken);

            logger.LogInformation("Gerando o conteúdo do newsletter...");
            var body = await newsletterGeneratorAgent.RunAsync(posts, cancellationToken);

            logger.LogInformation("Recuperando os assinantes...");

            var subscribers = await subscriberRepository.GetAllAsync(cancellationToken);

            logger.LogInformation("Enviando o newsletter para os assinantes...");
            foreach (var subscriber in subscribers)            
                await emailService.SendAsync(subscriber.Name, subscriber.Email, subject, body, cancellationToken);

            logger.LogInformation("Finalizado...");

        }
    }
}
