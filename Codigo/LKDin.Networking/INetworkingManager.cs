using System;
using System.Net.Sockets;

namespace LKDin.Networking
{
    public interface INetworkingManager
    {
        public Task<bool> InitTCPConnection();

        public void ShutdownTCPConnections();
    }
}

