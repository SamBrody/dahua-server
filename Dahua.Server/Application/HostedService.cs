namespace Dahua.Server.Application;

public class HostedService(IDahuaService dahuaService, ILogger<HostedService> logger) : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        logger.LogTrace("Background service starts");

        dahuaService.Start();
    }
}