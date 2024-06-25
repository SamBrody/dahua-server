using Dahua.Server.Model;
using Microsoft.AspNetCore.SignalR;

namespace Dahua.Server.Application;

public class EventHub(ILogger<EventHub> logger) : Hub {
    public async Task SendEvent(EventInfo eventInfo) {
        await Clients.Caller.SendAsync("Receive", eventInfo);
    }
}