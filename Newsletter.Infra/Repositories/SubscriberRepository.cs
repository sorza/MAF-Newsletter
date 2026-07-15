using Newsletter.Core.Models;
using Newsletter.Core.Repositories.Abstractions;

namespace Newsletter.Infra.Repositories
{
    public class SubscriberRepository : ISubscriberRepository
    {
        public async Task<IEnumerable<Subscriber>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(100, cancellationToken);

            return [
                new Subscriber("John Doe", "john.doe@exemple.com"),
                new Subscriber("Jane Doe", "jane.doe@google.com"),
                new Subscriber("John Smith", "john.smith@opus.com"),
                new Subscriber("Dotnet Bot", "donet@microsfot.com"),
                new Subscriber("Software Architect", "arch@tect.com")
            ];
        }
    }
}
