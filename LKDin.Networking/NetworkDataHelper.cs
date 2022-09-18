using System;
using System.Net.Sockets;
using System.Text;

namespace LKDin.Networking
{
    public class NetworkDataHelper
    {
        private readonly Socket _socket;

        private const int SIZE_DATA_LENGTH_INDICATOR = 4;

        public NetworkDataHelper(Socket socker)
        {
            _socket = socker;
        }

        public string ReceiveMessage()
        {
            byte[] dataLength = this.PerformReception(SIZE_DATA_LENGTH_INDICATOR);

            byte[] data = this.PerformReception(BitConverter.ToInt32(dataLength));

            string message = Encoding.UTF8.GetString(data);

            return message;
        }

        public void SendMessage(string messageBody)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(messageBody);

            byte[] messageSizeBytes = BitConverter.GetBytes(messageBytes.Length);

            this.PerformTransmission(messageSizeBytes);

            this.PerformTransmission(messageBytes);
        }

        private void PerformTransmission(byte[] data)
        {
            int offset = 0;
            int size = data.Length;
            while (offset < size)
            {
                int sent = _socket.Send(data, offset, size - offset, SocketFlags.None);

                if (sent == 0)
                {
                    throw new SocketException();
                }
                offset += sent;
            }
        }

        private byte[] PerformReception(int length)
        {
            byte[] response = new byte[length];

            int offset = 0;

            while (offset < length)
            {
                int received = _socket.Receive(response, offset, length - offset, SocketFlags.None);
                if (received == 0)
                {
                    throw new SocketException();
                }
                offset += received;
            }

            return response;
        }
    }
}

