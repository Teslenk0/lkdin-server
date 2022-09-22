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

        public override void InitSocketV4Connection()
        {
            this._socketV4 = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            var endpoint = new IPEndPoint(this.ServerIPAddress, this.ServerPort);

            this._socketV4.Bind(endpoint);

            this._socketV4.Listen(QUEUE_SIZE);

            this._isWorking = true;
        }

        public void AcceptSocketConnections(SocketConnectionHandler handler)
        {
            if(this._socketV4 != null)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Server escuchando en {0}:{1}", this.ServerIPAddress, this.ServerPort);
                Console.WriteLine("---------------------------------");

                while (this._isWorking)
                {
                    Socket clientSocket = this._socketV4.Accept();

                    new Thread(() => handler(clientSocket)).Start();
                }
            } else
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

