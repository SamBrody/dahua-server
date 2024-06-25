using Dahua.Server.Application;
using Dahua.Server.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSignalR(opt => {
    opt.EnableDetailedErrors = true;
});

builder.Services.Configure<DahuaOptions>(builder.Configuration.GetSection(DahuaOptions.Section));

builder.Services.AddHostedService<HostedService>();

builder.Services.AddSingleton<IDahuaService, DahuaService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<AlarmHub>("/alarm");

app.Run();