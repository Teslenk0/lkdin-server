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

        private ClientNetworkingManager() : base(ConfigNameSpace.CLIENT)
        { }

        public override void InitSocketV4Connection()
        {
            _socketV4 = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            var endpoint = new IPEndPoint(ServerIPAddress, ServerPort);

            _socketV4.Connect(endpoint);

            _isWorking = true;
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

