using System.Threading.Channels;
using Dahua.Server.Application;
using Dahua.Server.Model;
using Dahua.Server.Presentation;
using Microsoft.AspNetCore.SignalR;

namespace Dahua.Server;

public class HostedService(
    ChannelReader<EventInfo> reader,
    IHubContext<EventHub> hub,
    IDahuaService dahuaService,
    ILogger<HostedService> logger
) : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        dahuaService.Start();
        
        logger.LogTrace("Background service running");

        while (!stoppingToken.IsCancellationRequested) {
            var item = await reader.ReadAsync(stoppingToken);

            await hub.Clients.All.SendAsync("Receive", item, stoppingToken);
            
            logger.LogTrace($"Send event {item.EventId} to clients");
        }
    }
}