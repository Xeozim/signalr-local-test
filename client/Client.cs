using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRLocalTest.Client;

public class Client
{
    public static async Task RunAsync(string[] args)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/clockHub")
            .Build();

        connection.On<string>("ReceiveTime", (time) =>
        {
            Console.WriteLine($"Current time from server: {time}");
        });

        try
        {
            await connection.StartAsync();
            Console.WriteLine("Connected to the SignalR hub.");
            Console.WriteLine("Press any key to exit.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting connection: {ex.Message}");
        }

        Console.ReadKey();

        await connection.DisposeAsync();
    }
}
