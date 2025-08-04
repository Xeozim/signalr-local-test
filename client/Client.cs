using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRLocalTest.Client;

public class Client
{
    public static async Task RunAsync(string[] args)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/clockHub")
            .Build();

        // This handler receives the time updates from the server's background worker
        connection.On<string>("ReceiveTime", (time) =>
        {
            // To prevent the time update from overwriting user input, we print it on a new line.
            Console.WriteLine($"\n[Server Time]: {time}");
        });

        // This handler receives broadcast messages from other clients
        connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            Console.WriteLine($"\n[{user}]: {message}");
        });

        try
        {
            await connection.StartAsync();
            Console.WriteLine("Connected to the SignalR hub. Type a message and press Enter to send. Type 'exit' to quit.");

            // Get a username for this client session
            Console.Write("Please enter your name: ");
            var user = Console.ReadLine();

            // Loop to read console input and send messages
            var message = string.Empty;
            while (!string.Equals(message = Console.ReadLine(), "exit", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(message)) continue;

                // Call the "SendMessage" method on the hub
                await connection.InvokeAsync("SendMessage", user, message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting or sending message: {ex.Message}");
        }
        finally
        {
            // Ensure the connection is closed gracefully
            if (connection.State == HubConnectionState.Connected)
            {
                await connection.DisposeAsync();
            }
        }
    }
}
