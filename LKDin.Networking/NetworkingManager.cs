using System;
using System.Net.Sockets;

namespace LKDin.Networking
{
    public abstract class NetworkingManager : INetworkingManager
    {
        protected Socket _socketV4;

        protected bool _isWorking;

        public abstract void InitSocketV4Connection(string ipAddress, int port, int backlog);

        public bool IsSocketConnected()
        {
            bool part1 = this._socketV4.Poll(1000, SelectMode.SelectRead);
            bool part2 = (this._socketV4.Available == 0);
            if ((part1 && part2) || !this._socketV4.Connected)
                return false;
            else
                return true;
        }

        public void ShutdownSocketConnections()
        {
            if (this._socketV4 != null && IsSocketConnected())
            {
                this._isWorking = false;

                this._socketV4.Shutdown(SocketShutdown.Both);

                this._socketV4.Close();
            }
        }
    }
}

