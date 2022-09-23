using System.Net;
using System.Net.Sockets;
using LKDin.Helpers.Configuration;
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

        private ClientNetworkingManager() : base(ConfigNameSpace.CLIENT)
        { }

        public override bool InitSocketV4Connection()
        {
            int retries = 0;

            _socketV4 = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            var endpoint = new IPEndPoint(ServerIPAddress, ServerPort);

            while (!this._isWorking && retries < MAX_RETRIES)
            {
                try
                {
                    retries++;

                    _socketV4.Connect(endpoint);

                    _isWorking = true;
                }
                catch (Exception e)
                {
                    if(retries < MAX_RETRIES)
                    {
                        Console.WriteLine("Falló la conexión al servidor => IP = {0} | PUERTO = {1}", this.ServerIPAddress, this.ServerPort);
                        Console.WriteLine("Reintentando en {0}ms...", TIME_BETWEEN_RETRIES_MS);
                        Thread.Sleep(TIME_BETWEEN_RETRIES_MS);
                    } else
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
    }
}

