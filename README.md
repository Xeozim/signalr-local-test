# SignalR Local Test

A simple SignalR application demonstrating real-time communication between a server and client. The server broadcasts the current time every 5 seconds to all connected clients.

## Project Structure

```text
signalr-local-test/
├── server/              # SignalR server application
│   ├── Program.cs       # Server implementation with ClockHub and ClockWorker
│   └── server.csproj    # Server project file
├── client/              # SignalR client application
│   ├── Program.cs       # Client implementation
│   └── client.csproj    # Client project file
└── signalr-local-test.sln  # Solution file
```

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Any text editor or IDE (Visual Studio, Visual Studio Code, JetBrains Rider, etc.)

## Setup

1. **Clone or download the repository**

   ```bash
   git clone <repository-url>
   cd signalr-local-test
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

## Running the Application

### Start the Server

```bash
dotnet run --project server
```

The server will start and listen on `http://localhost:5000`. You should see:

```text
SignalR Server is running.
```

### Start the Client

In a new terminal window:

```bash
dotnet run --project client
```

The client will connect to the server and start receiving time updates every 5 seconds.

## How It Works

### Server (`server/Program.cs`)

- **ClockHub**: A SignalR hub that handles client connections
- **ClockWorker**: A background service that broadcasts the current time every 5 seconds
- **Endpoints**:
  - `/clockHub` - SignalR hub endpoint
  - `/` - Simple status page

### Client (`client/Program.cs`)

- Connects to the SignalR hub at `http://localhost:5000/clockHub`
- Listens for `ReceiveTime` messages from the server
- Displays received time updates in the console

## Troubleshooting

### Common Issues

1. **Port already in use**: If port 5000 is already in use, you can specify a different port:

   ```bash
   dotnet run --project server --urls "http://localhost:5001"
   ```

   Then update the client connection URL accordingly.

2. **Connection refused**: Make sure the server is running before starting the client.

3. **Build errors**: Ensure you have .NET 8.0 SDK installed:

   ```bash
   dotnet --version
   ```

### Testing the Connection

You can test if the server is running by visiting `http://localhost:5000` in your web browser. You should see "SignalR Server is running."
