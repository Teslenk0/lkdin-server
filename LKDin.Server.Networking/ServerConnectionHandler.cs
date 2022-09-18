using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using LKDin.Helpers.Configuration;
using LKDin.Networking;

namespace LKDin.Server.Networking
{
    public delegate void SocketConnectionHandler(Socket socket);

    public static class ServerConnectionHandler
    {
        public static void HandleConnection(Socket clientSocket)
        {
            bool clientIsConnected = true;

            NetworkDataHelper networkDataHelper = new NetworkDataHelper(clientSocket);

            while (clientIsConnected)
            {
                try
                {
                    string message = networkDataHelper.ReceiveMessage();

                    Console.WriteLine(message);

                    string response = $"Message '{message}' received successfully";

                    networkDataHelper.SendMessage(response);
                }
                catch (SocketException)
                {
                    Console.WriteLine("Client disconnected");
                    clientIsConnected = false;
                }
            }
        }
    }
}

