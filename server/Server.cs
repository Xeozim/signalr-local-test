using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SignalR;

namespace SignalRLocalTest.Server;

// Defines the SignalR hub
public class ClockHub : Hub
{
    // This method can be called by clients, but is currently unused.
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}

public class ClockWorker(IHubContext<ClockHub> hubContext) : BackgroundService
{
    private readonly IHubContext<ClockHub> _hubContext = hubContext;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveTime", DateTime.Now.ToString("h:mm:ss tt"), cancellationToken: stoppingToken);
            await Task.Delay(60000, stoppingToken);
        }
    }
}

public class Server
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddSignalR();
                    services.AddHostedService<ClockWorker>();
                });

                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<ClockHub>("/clockHub");
                        endpoints.MapGet("/", async context =>
                        {
                            await context.Response.WriteAsync("SignalR Server is running.");
                        });
                    });
                });
            });

    public static void Run(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }
}
