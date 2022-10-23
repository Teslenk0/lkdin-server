using System.Net;
using System.Net.Sockets;
using LKDin.Networking;

namespace LKDin.Client.Networking
{
    // [SINGLETON]
    public sealed class ClientNetworkingManager : NetworkingManager, INetworkingManager
    {
        private static readonly object SingletonSyncRoot = new();

        private static volatile ClientNetworkingManager _instance;

        private const int MAX_RETRIES = 5;

        private const int TIME_BETWEEN_RETRIES_MS = 2000;

        private TcpClient _tcpClient;

        private ClientNetworkingManager() : base()
        { }

        public override async Task<bool> InitTCPConnection()
        {
            int retries = 0;

            var endpoint = new IPEndPoint(ServerIPAddress, ServerPort);

            _tcpClient = new TcpClient();

            while (!this._isWorking && retries < MAX_RETRIES)
            {
                try
                {
                    retries++;

                    await _tcpClient.ConnectAsync(endpoint);

                    _isWorking = true;
                }
                catch (Exception e)
                {
                    if (retries < MAX_RETRIES)
                    {
                        Console.WriteLine("Falló la conexión al servidor => IP = {0} | PUERTO = {1}", this.ServerIPAddress, this.ServerPort);
                        Console.WriteLine("Reintentando en {0}ms...", TIME_BETWEEN_RETRIES_MS);
                        Thread.Sleep(TIME_BETWEEN_RETRIES_MS);
                    }
                    else
                    {
                        Console.WriteLine("Limite de intentos alcanzado.");
                        Console.WriteLine("Error: {0}", e.Message);
                    }
                }
            }

            return this._isWorking;
        }

        public static ClientNetworkingManager Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                lock (SingletonSyncRoot)
                {
                    return _instance ??= new ClientNetworkingManager();
                }
            }
        }

        public TcpClient GetClient()
        {
            return this._tcpClient;
        }

        public override bool IsConnected()
        {
            var rawSocket = _tcpClient.Client;

            bool part1 = rawSocket.Poll(1000, SelectMode.SelectRead);

            bool part2 = (rawSocket.Available == 0);

            if ((part1 && part2) || !rawSocket.Connected)
                return false;
            else
                return true;
        }

        public override void ShutdownTCPConnections()
        {
            if (_tcpClient != null && IsConnected())
            {
                _tcpClient.Close();
            }

            _isWorking = false;
        }
    }
}

