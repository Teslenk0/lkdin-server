using System.Net;
using System.Net.Sockets;
using System.Text;
using LKDin.Helpers.Configuration;
using LKDin.IUI;
using LKDin.Networking;
using LKDin.Server.BusinessLogic;
using LKDin.Server.Networking;
using LKDin.UI.ConsoleMenu;
using LKDin.UI.ConsoleMenu.AvailableOptions;

//Console.WriteLine("Starting Client Application..");

//var socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//var remoteEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);

//Console.WriteLine("Connecting to server...");
//socketClient.Connect(remoteEndpoint);

//Console.WriteLine("Connected to server!!!!");
//Console.WriteLine("Type a message and press enter to send it");

//bool clientRunning = true;

//var networkDataHelper = new NetworkDataHelper(socketClient);

//while (clientRunning)
//{
//    string message = Console.ReadLine();

//    if (message.Equals("exit"))
//    {
//        clientRunning = false;
//    }

//    else
//    {
//        try
//        {
//            networkDataHelper.SendMessage(message, AvailableOperation.ACK);

//            var response = networkDataHelper.ReceiveMessage();

//            Console.WriteLine($"Server says: {response}");
//        }
//        catch (SocketException)
//        {
//            Console.WriteLine("Connection with the server has been interrupted");
//            clientRunning = false;
//        }
//    }
//}
//Console.WriteLine("Will Close Connection...");

//socketClient.Shutdown(SocketShutdown.Both);

//socketClient.Close();


namespace LKDin.Client;

public class LKDinClient
{
    public static void Main()
    {
        var networkingManager = ClientNetworkingManager.Instance;

        networkingManager.InitSocketV4Connection();

        var socketClient = networkingManager.GetSocket();

        var networkDataHelper = new NetworkDataHelper(socketClient);

        var userService = new UserClientService(networkDataHelper);

        var enabledOptions = new List<IMenuOption>()
        {
            new CreateUserOption("Crear Usuario", userService),
            new ExitOption("Salir", networkingManager)
        };

        IUIService uiService = new ConsoleMenuService(enabledOptions);

        uiService.Render();
    }
}
