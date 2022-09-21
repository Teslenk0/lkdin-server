using LKDin.Helpers.Configuration;
using System;
using System.Net;
using System.Net.Sockets;

namespace LKDin.Networking
{
    public abstract class NetworkingManager : INetworkingManager
    {
        protected Socket _socketV4;

        protected bool _isWorking;

        protected readonly int ServerPort;

        protected readonly IPAddress ServerIPAddress;

        protected NetworkingManager(ConfigNameSpace configNameSpace)
        {
            this.ServerPort = int.Parse(ConfigManager.GetConfig("SERVER_PORT", configNameSpace) ?? "5000");

            this.ServerIPAddress = IPAddress.Parse(ConfigManager.GetConfig("SERVER_IP", configNameSpace) ?? "127.0.0.1");
        }

        public abstract void InitSocketV4Connection();

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

        public Socket GetSocket()
        {
            return this._socketV4;
        }
    }
}

