using System.Net;
using System.Net.Sockets;
using LKDin.Exceptions;
using LKDin.Logging.Client;
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

        private readonly Logger _logger;

        private ServerNetworkingManager() : base()
        {
            _logger = new Logger("server-v2:networking-manager");
        }

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
                _logger.Error($"Falló la inicialización del socket server => IP = {this.ServerIPAddress} | PUERTO = {this.ServerPort}");
                _logger.Error($"Error: {e.Message}");
            }

            return this._isWorking;
        }

        public async Task AcceptTCPConnections(TCPConnectionHandler handler)
        {
            if (this._tcpListener != null)
            {
                Console.WriteLine("---------------------------------");
                _logger.Info($"Socket Server escuchando en {this.ServerIPAddress}:{this.ServerPort}");
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

