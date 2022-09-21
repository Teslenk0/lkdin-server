using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.Helpers.Serialization;
using LKDin.Helpers.Utils;
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

        public const string LENGTH_HEADER_NAME = "LENGTH";

        public const string CMD_HEADER_NAME = "CMD";

        public const string MSG_NAME = "MESSAGE";

        public NetworkDataHelper(Socket socket)
        {
            _socket = socket;
        }

        public void ThrowException(string exceptionMessage)
        {
            var exceptionDTO = SerializationManager.Deserialize<ExceptionDTO>(exceptionMessage);

            var assemblyName = exceptionDTO.AssemblyName.Replace("#", "=");

            Exception e = null;

            var availableTypes = new Dictionary<Type, Action> {
                { typeof(AssetDoesNotExistException), () => { e = new AssetDoesNotExistException(exceptionDTO.Message, true); } },
                { typeof(CommandNotSupportedException), () => { e = new CommandNotSupportedException(exceptionDTO.Message, true); } },
                { typeof(EntityAlreadyExistsException), () => { e = new EntityAlreadyExistsException(exceptionDTO.Message, true); } },
                { typeof(UnauthorizedException), () => { e = new UnauthorizedException(); } },
                { typeof(UserAlreadyExistsException), () => { e = new UserAlreadyExistsException(exceptionDTO.Message, true); } },
                { typeof(UserDoesNotExistException), () => { e = new UserDoesNotExistException(exceptionDTO.Message, true); } },
                { typeof(WorkProfileAlreadyExistsException), () => { e = new WorkProfileAlreadyExistsException(exceptionDTO.Message, true); } },
                { typeof(WorkProfileDoesNotExistException), () => { e = new WorkProfileDoesNotExistException(exceptionDTO.Message, true); } },
            };

            var type = Type.GetType($"{exceptionDTO.ExceptionType},{assemblyName}");

            if (availableTypes.ContainsKey(type))
            {
               availableTypes[type]();

            } else
            {
                e = new Exception(exceptionDTO.Message);
            }
           
            if(e != null)
            {
                throw e;
            }
            
        }

        public Dictionary<string, string> ReceiveMessage()
        {
            var resultantData = new Dictionary<string, string>();

            byte[] rawHeaders = this.PerformReception(HEADER_SIZE);

            var parsedHeaders = this.DeserializeHeaders(rawHeaders);

            byte[] data = this.PerformReception(int.Parse(parsedHeaders[LENGTH_HEADER_NAME]));

            var message = this.DeserializeMessage(data);

            if ((AvailableOperation)int.Parse(parsedHeaders[CMD_HEADER_NAME] ?? "01") == AvailableOperation.ERR)
            {
                this.ThrowException(message);
            }

            resultantData.Add(MSG_NAME, message);

            resultantData.Add(CMD_HEADER_NAME, parsedHeaders[CMD_HEADER_NAME]);

            return resultantData;
        }

        public void SendMessage(string messageBody, AvailableOperation availableOperation)
        {
            byte[] messageBytes = this.SerializeMessage(messageBody);

            var messageBodySize = messageBytes.Length.ToString();

            while (messageBodySize.Length != SIZE_LENGTH_HEADER)
            {
                messageBodySize = $"0{messageBodySize}";
            }

            var operation = ((int)availableOperation).ToString();

            while (operation.Length != SIZE_CMD_HEADER)
            {
                operation = $"0{operation}";
            }

            var headers = $"CMD={operation}|LENGTH={messageBodySize}";

            byte[] rawHeaders = this.SerializeMessage(headers);

            this.PerformTransmission(rawHeaders);

            this.PerformTransmission(messageBytes);
        }

        public void SendException(Exception exception)
        {
            var message = new ExceptionDTO()
            {
                Message = exception.Message,
                ExceptionType = exception.GetType().FullName,
                AssemblyName = exception.GetType().Assembly.FullName.Replace("=", "#")
            };

            var stringifiedMessage = SerializationManager.Serialize<ExceptionDTO>(message);

            byte[] messageBytes = this.SerializeMessage(stringifiedMessage);

            var messageBodySize = messageBytes.Length.ToString();

            while (messageBodySize.Length != SIZE_LENGTH_HEADER)
            {
                messageBodySize = $"0{messageBodySize}";
            }

            var operation = ((int)AvailableOperation.ERR).ToString();

            while (operation.Length != SIZE_CMD_HEADER)
            {
                operation = $"0{operation}";
            }

            var headers = $"CMD={operation}|LENGTH={messageBodySize}";

            byte[] rawHeaders = this.SerializeMessage(headers);

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

