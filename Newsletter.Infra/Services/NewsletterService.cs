using Newsletter.Core.Services.Abstractions;

namespace Newsletter.Infra.Services
{
    internal class NewsletterService : INewsletterService
    {
        public Task SendAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
