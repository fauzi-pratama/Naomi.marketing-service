using Naomi.marketing_service.Services.PromotionService;

namespace Naomi.marketing_service.Configurations
{
    public class PromotionStatusJob : BackgroundService
    {
        private readonly IServiceScopeFactory _factory;
        private readonly ILogger<PromotionStatusJob> _logger;

        public PromotionStatusJob(IServiceScopeFactory factory, ILogger<PromotionStatusJob> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                IPromotionService jobManager = asyncScope.ServiceProvider.GetRequiredService<IPromotionService>();
                try
                {
                    _logger.LogInformation("Promotion Status job started");
                    await jobManager.UpdatePromotionStatusByDate();
                    _logger.LogInformation("Promotion Status job completed");
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    _logger.LogError("Promotion status job error: {msg}", msg);
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
