using System;
using System.Net;
using System.Net.Sockets;
using LKDin.Exceptions;
using LKDin.Networking;

namespace LKDin.Server.Networking
{
    // [SINGLETON]
    public sealed class ServerNetworkingManager : NetworkingManager, INetworkingManager
    {
        private static readonly object SingletonSyncRoot = new object();

        private static volatile ServerNetworkingManager _instance;

        private ServerNetworkingManager()
        { }

        public override void InitSocketV4Connection(string ipAddress, int port, int backlog)
        {
            this._socketV4 = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            var endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            this._socketV4.Bind(endpoint);

            this._socketV4.Listen(backlog);

            this._isWorking = true;
        }

        public void AcceptSocketConnections(SocketConnectionHandler handler)
        {
            if(this._socketV4 != null)
            {
                while (this._isWorking)
                {
                    Socket clientSocket = this._socketV4.Accept();

                    new Thread(() => handler(clientSocket));
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
                    return _instance ?? (_instance = new ServerNetworkingManager());
                }
            }
        }
    }
}

