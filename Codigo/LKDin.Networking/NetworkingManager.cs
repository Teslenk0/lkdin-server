using LKDin.Helpers.Configuration;
using System.Net;
using System.Net.Sockets;

namespace LKDin.Networking
{
    public delegate void SocketShutdownHandler();

    public abstract class NetworkingManager : INetworkingManager
    {
        private const int VALIDATE_CONNECTION_INTERVAL_MS = 1000;

        protected Socket _socketV4;

        protected bool _isWorking;

        protected readonly int ServerPort;

        protected readonly IPAddress ServerIPAddress;

        protected NetworkingManager()
        {
            try
            {
                var rawPort = ConfigManager.GetConfig(ConfigManager.SERVER_PORT_KEY);

                // Defaulting in case it's null
                this.ServerPort = int.Parse(string.IsNullOrWhiteSpace(rawPort) ? "5000" : rawPort);

                var rawIp = ConfigManager.GetConfig(ConfigManager.SERVER_IP_KEY);

                // Defaulting in case it's null
                this.ServerIPAddress = IPAddress.Parse(string.IsNullOrWhiteSpace(rawIp) ? "127.0.0.1" : rawIp);
            }
            catch (Exception)
            {
                Console.WriteLine($"Error al cargar {ConfigManager.SERVER_IP_KEY} y {ConfigManager.SERVER_PORT_KEY}");
            }
            
        }

        public abstract bool InitSocketV4Connection();

        public void ValidateConnectionOrShutDown(SocketShutdownHandler handler)
        {
            while (this._isWorking)
            {
                if (!this.IsSocketConnected())
                {
                    Console.Clear();

                    Console.WriteLine("Se cerró la conexión al servidor => IP = {0} | PUERTO = {1}", this.ServerIPAddress, this.ServerPort);

                    handler();
                } else
                {
                    Thread.Sleep(VALIDATE_CONNECTION_INTERVAL_MS);
                }
            }
        }

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
                this._socketV4.Shutdown(SocketShutdown.Both);

                this._socketV4.Close();
            }

            this._isWorking = false;
        }

        public Socket GetSocket()
        {
            return this._socketV4;
        }
    }
}

