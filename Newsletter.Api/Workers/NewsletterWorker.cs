using Newsletter.Core.Services.Abstractions;

namespace Newsletter.Api.Workers
{
    public class NewsletterWorker(
        ILogger<NewsletterWorker> logger, 
        IServiceScopeFactory scopeFactory) : BackgroundService
    {

        private readonly TimeSpan _scheduleTime = new TimeSpan(8, 0, 0); // 8:00 AM
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("-> Newsletter worker iniciado...");

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRun = GetNextSunday(now);

                //var delay = nextRun - now;
                var delay = TimeSpan.FromSeconds(10); // For testing purposes, run every 10 seconds
                logger.LogInformation($"-> Próxima execução em: {nextRun} (em {delay.TotalSeconds} segundos)");

                try
                {                   
                    logger.LogInformation($"-> Executando o trabalho agendado às {DateTime.Now}");
                    await DoWorkAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                await Task.Delay(delay, stoppingToken);

            }
        }

        private DateTime GetNextSunday(DateTime current)
        {
            var daysUntilSunday = ((int)DayOfWeek.Sunday - (int)current.DayOfWeek + 7) % 7;

            var nextSunday = current.AddDays(daysUntilSunday).Add(_scheduleTime);

            if (nextSunday <= current)
                nextSunday = nextSunday.AddDays(7);

            return nextSunday;
        }

        private async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("-> Working...");
            using var scope = scopeFactory.CreateScope();
            var newsletterService = scope.ServiceProvider.GetRequiredService<INewsletterService>();
            await newsletterService.SendAsync(cancellationToken);
        }
    }
}
