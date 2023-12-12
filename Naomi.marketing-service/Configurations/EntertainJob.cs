using DotNetCore.CAP;
using Naomi.marketing_service.Services.EntertainService;
using Naomi.marketing_service.Services.PromotionService;

namespace Naomi.marketing_service.Configurations
{
    public class EntertainJob : BackgroundService
    {
        private readonly IServiceScopeFactory _factory;
        private readonly ILogger<EntertainJob> _logger;


        public EntertainJob(IServiceScopeFactory factory, ILogger<EntertainJob> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromHours(1));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                // do async work
                // ...as above
                if (DateTime.UtcNow.Day == 1 && DateTime.UtcNow.Hour == 0)
                {
                    await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                    IPromotionService jobPromoService = asyncScope.ServiceProvider.GetRequiredService<IPromotionService>();
                    IEntertainService jobEntertainService = asyncScope.ServiceProvider.GetRequiredService<IEntertainService>();
                    try
                    {
                        _logger.LogInformation("Auto create entertain budget started");
                        await jobEntertainService.CreateEntertainBudgetAuto();
                        _logger.LogInformation("Auto create entertain budget completed");

                        _logger.LogInformation("Auto create promo entertain started");
                        await jobPromoService.CreatePromoEntertainAuto();
                        _logger.LogInformation("Auto create promo entertain completed");

                        _logger.LogInformation("Auto publish promo entertain started");
                        await jobPromoService.PublishPromoEntertainAuto();
                        _logger.LogInformation("Auto publish promo entertain completed");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Failed processing Entertain job: {ex.Message}", ex.Message);
                    }
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
