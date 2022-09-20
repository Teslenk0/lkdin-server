using System;
using System.Net.Sockets;
using System.Text;

namespace LKDin.Networking
{
    public class NetworkDataHelper
    {
        private readonly Socket _socket;

        private const int SIZE_LENGTH_HEADER = 10;

        private const int SIZE_CMD_HEADER = 4;

        // HEADER => "CMD=1111|LENGTH=1234567890"
        private const int HEADER_SIZE = 26;

        private const string LENGTH_HEADER_NAME = "LENGTH";

        private const string CMD_HEADER_NAME = "CMD";

        public NetworkDataHelper(Socket socket)
        {
            _socket = socket;
        }

        public string ReceiveMessage()
        {
            byte[] rawHeaders = this.PerformReception(HEADER_SIZE);

            var parsedHeaders = this.DeserializeHeaders(rawHeaders);

            byte[] data = this.PerformReception(int.Parse(parsedHeaders[LENGTH_HEADER_NAME]));

            return this.DeserializeMessage(data);
        }

        public void SendMessage(string messageBody, AvailableOperation availableOperation)
        {
            var messageBodySize = messageBody.Length.ToString();

            while (messageBodySize.Length != SIZE_LENGTH_HEADER)
            {
                messageBodySize = $"0{messageBodySize}";
            }

            var operation = availableOperation.ToString();

            while (operation.Length != SIZE_CMD_HEADER)
            {
                operation = $"0{operation}";
            }

            byte[] rawHeaders = this.SerializeMessage($"CMD={operation}|LENGTH={messageBodySize}");

            byte[] messageBytes = this.SerializeMessage(messageBody);

            this.PerformTransmission(rawHeaders);

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

        private byte[] SerializeMessage(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }

        private string DeserializeMessage(byte[] rawMessage)
        {
            string message = Encoding.UTF8.GetString(rawMessage);

            return message;
        }

        private Dictionary<string, string> DeserializeHeaders(byte[] rawHeaders)
        {
            string headers = this.DeserializeMessage(rawHeaders);

            var results = new Dictionary<string, string>();

            var fields = headers.Split('|');

            foreach (string field in fields)
            {
                var data = field.Split('=');

                results[data[0]] = data[1];
            }

            return results;
        }
    }
}

