using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services.ModbusServices;
public class EnergyDataBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<EnergyDataBackgroundService> _logger;
    
    public EnergyDataBackgroundService(IServiceProvider services, ILogger<EnergyDataBackgroundService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EnergyDataBackgroundService başlatıldı. 2 dakika aralıklarla çalışacak.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _services.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<EnergyDataService>();

                var sw = System.Diagnostics.Stopwatch.StartNew();

                await service.ReadAllGatewaysAsync(stoppingToken);

                sw.Stop();

                _logger.LogInformation("Tüm gateway okuma süresi: {TotalSeconds:F2} saniye", sw.Elapsed.TotalSeconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BackgroundService çalışırken hata oluştu");
            }

            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
        }
    }
}