using System.Net;
using System.Net.Sockets;
using System.Text;
using LKDin.Networking;

Console.WriteLine("Starting Client Application..");

var socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
var localEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
var remoteEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);

socketClient.Bind(localEndpoint);
Console.WriteLine("Connecting to server...");
socketClient.Connect(remoteEndpoint);

Console.WriteLine("Connected to server!!!!");
Console.WriteLine("Type a message and press enter to send it");

bool clientRunning = true;

var networkDataHelper = new NetworkDataHelper(socketClient);

while (clientRunning)
{
    string message = Console.ReadLine();

    if (message.Equals("exit"))
    {
        clientRunning = false;
    }

    else
    {
        try
        {
            networkDataHelper.SendMessage(message);

            var response = networkDataHelper.ReceiveMessage();

            Console.WriteLine($"Server says: {response}");
        }
        catch (SocketException)
        {
            Console.WriteLine("Connection with the server has been interrupted");
            clientRunning = false;
        }
    }
}
Console.WriteLine("Will Close Connection...");

socketClient.Shutdown(SocketShutdown.Both);

socketClient.Close();