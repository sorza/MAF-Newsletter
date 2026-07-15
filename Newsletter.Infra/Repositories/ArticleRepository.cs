using Newsletter.Core.Models;
using Newsletter.Core.Repositories.Abstractions;

namespace Newsletter.Infra.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        public async Task<IEnumerable<Article>> GetFromLastWeekAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(100, cancellationToken);

            return [
                new Article("Article 1", "https://example.com/article1", "Content of article 1", DateTime.UtcNow.AddDays(-2)),
                new Article("Article 2", "https://example.com/article2", "Content of article 2", DateTime.UtcNow.AddDays(-5)),
                new Article("Article 3", "https://example.com/article3", "Content of article 3", DateTime.UtcNow.AddDays(-7)),
                new Article("Article 4", "https://example.com/article4", "Content of article 4", DateTime.UtcNow.AddDays(-10)),
                new Article("Article 5", "https://example.com/article5", "Content of article 5", DateTime.UtcNow.AddDays(-12))
            ];
        }
    }
}
