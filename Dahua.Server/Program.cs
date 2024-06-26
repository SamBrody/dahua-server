using System.Threading.Channels;
using Dahua.Server;
using Dahua.Server.Application;
using Dahua.Server.Configuration;
using Dahua.Server.Model;
using Dahua.Server.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSignalR();

builder.Services.Configure<DahuaOptions>(builder.Configuration.GetSection(DahuaOptions.Section));

builder.Services.AddHostedService<HostedService>();

builder.Services.AddSingleton<IDahuaService, DahuaService>();

// Register Channel
builder.Services.AddSingleton(Channel.CreateUnbounded<EventInfo>(new UnboundedChannelOptions { SingleWriter = true, SingleReader = true}));
builder.Services.AddSingleton<ChannelReader<EventInfo>>(svc => svc.GetRequiredService<Channel<EventInfo>>().Reader);
builder.Services.AddSingleton<ChannelWriter<EventInfo>>(svc => svc.GetRequiredService<Channel<EventInfo>>().Writer);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<EventHub>("/event");

app.Run();