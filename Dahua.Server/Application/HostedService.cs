using Microsoft.AspNetCore.SignalR;

namespace Dahua.Server.Application;

public class HostedService(IHubContext<EventHub> hub, IDahuaService dahuaService, ILogger<HostedService> logger) : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        logger.LogInformation("HostedService execute");

        dahuaService.Start();
    }
}