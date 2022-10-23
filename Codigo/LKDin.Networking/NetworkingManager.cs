﻿using LKDin.Helpers.Configuration;
using System.Net;
using System.Net.Sockets;

namespace LKDin.Networking
{
    public delegate Task SocketShutdownHandler();

    public abstract class NetworkingManager : INetworkingManager
    {
        private const int VALIDATE_CONNECTION_INTERVAL_MS = 1000;

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

        public abstract Task<bool> InitTCPConnection();

        public abstract bool IsConnected();

        public abstract void ShutdownTCPConnections();

        public async Task ValidateConnectionOrShutDown(SocketShutdownHandler handler)
        {
            while (this._isWorking)
            {
                var isConnected = IsConnected();

                if (!isConnected)
                {
                    Console.Clear();

                    Console.WriteLine("Se cerró la conexión al servidor => IP = {0} | PUERTO = {1}", this.ServerIPAddress, this.ServerPort);

                    await handler();
                } else
                {
                    await Task.Delay(VALIDATE_CONNECTION_INTERVAL_MS);
                }
            }
        }
    }
}

