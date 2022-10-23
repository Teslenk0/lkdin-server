using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using LKDin.Exceptions;
using LKDin.Networking;

namespace LKDin.Server.Networking
{
    // [SINGLETON]
    public sealed class ServerNetworkingManager : NetworkingManager, INetworkingManager
    {
        private const int QUEUE_SIZE = 100;

        private static readonly object SingletonSyncRoot = new();

        private static volatile ServerNetworkingManager _instance;

        private TcpListener _tcpListener;

        private ServerNetworkingManager() : base()
        { }

        public override async Task<bool> InitTCPConnection()
        {
            var endpoint = new IPEndPoint(this.ServerIPAddress, this.ServerPort);

            this._tcpListener = new TcpListener(endpoint);

            try
            {
                this._tcpListener.Start(QUEUE_SIZE);

                this._isWorking = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Falló la inicialización del servidor => IP = {0} | PUERTO = {1}", this.ServerIPAddress, this.ServerPort);
                Console.WriteLine("Error: {0}", e.Message);
            }

            return this._isWorking;
        }

        public async Task AcceptTCPConnections(TCPConnectionHandler handler)
        {
            if (this._tcpListener != null)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Server escuchando en {0}:{1}", this.ServerIPAddress, this.ServerPort);
                Console.WriteLine("---------------------------------");

                while (this._isWorking)
                {
                    var tcpClientSocket = await this._tcpListener.AcceptTcpClientAsync().ConfigureAwait(false);

                    Task.Run(
                        async () => await handler(tcpClientSocket)
                        .ConfigureAwait(false)
                    );
                }
            }
            else
            {
                throw new TCPConnectionInitializationException();
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

        public override bool IsConnected()
        {
            return this._isWorking;
        }

        public override void ShutdownTCPConnections()
        {
            if (_tcpListener != null && IsConnected())
            {
                _tcpListener.Stop();
            }

            _isWorking = false;
        }
    }
}

