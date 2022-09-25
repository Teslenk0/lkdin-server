using System;
using System.Net.Sockets;

namespace LKDin.Networking
{
    public interface INetworkingManager
    {
        public bool InitSocketV4Connection();

        public void ShutdownSocketConnections();
    }
}

