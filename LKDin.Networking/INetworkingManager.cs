using System;
namespace LKDin.Networking
{
    public interface INetworkingManager
    {
        public void InitSocketV4Connection(string ipAddress, int port, int backlog);

        public void ShutdownSocketConnections();
    }
}

