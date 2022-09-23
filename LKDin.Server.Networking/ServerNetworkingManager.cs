using System.Net;
using System.Net.Sockets;
using LKDin.Exceptions;
using LKDin.Helpers.Configuration;
using LKDin.Networking;

namespace LKDin.Server.Networking
{
    // [SINGLETON]
    public sealed class ServerNetworkingManager : NetworkingManager, INetworkingManager
    {
        private const int QUEUE_SIZE = 100;

        private static readonly object SingletonSyncRoot = new object();

        private static volatile ServerNetworkingManager _instance;

        private ServerNetworkingManager() : base(ConfigNameSpace.SERVER)
        { }

        public override bool InitSocketV4Connection()
        {
            this._socketV4 = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            var endpoint = new IPEndPoint(this.ServerIPAddress, this.ServerPort);

            try
            {
                this._socketV4.Bind(endpoint);

                this._socketV4.Listen(QUEUE_SIZE);

                this._isWorking = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Falló la inicialización del servidor => IP = {0} | PUERTO = {1}", this.ServerIPAddress, this.ServerPort);
                Console.WriteLine("Error: {0}", e.Message);
            }

            return this._isWorking;
        }

        public void AcceptSocketConnections(SocketConnectionHandler handler)
        {
            if (this._socketV4 != null)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Server escuchando en {0}:{1}", this.ServerIPAddress, this.ServerPort);
                Console.WriteLine("---------------------------------");

                while (this._isWorking)
                {
                    Socket clientSocket = this._socketV4.Accept();

                    new Thread(() => handler(clientSocket)).Start();
                }
            }
            else
            {
                throw new SocketInitializationException();
            }
        }

        public static ServerNetworkingManager Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                lock (SingletonSyncRoot)
                {
                    return _instance ??= new ServerNetworkingManager();
                }
            }
        }
    }
}

