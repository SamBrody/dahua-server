using Microsoft.AspNetCore.SignalR;

namespace Dahua.Server.Application;

public interface IAlarmClient {
    Task ReceiveUpdates(ICollection<string> alarms);
}

public class AlarmHub(ILogger<AlarmHub> logger) : Hub<IAlarmClient> {
    public async Task Send(ICollection<string> alarms) {
        await Clients.Caller.ReceiveUpdates(alarms);
    }
}